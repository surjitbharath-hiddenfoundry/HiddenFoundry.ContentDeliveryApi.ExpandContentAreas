using EPiServer;
using EPiServer.ContentApi.Core.Serialization;
using EPiServer.ContentApi.Core.Serialization.Models;
using EPiServer.Core;
using EPiServer.Core.Html.StringParsing;
using EPiServer.Security;
using EPiServer.ServiceLocation;
using EPiServer.SpecializedProperties;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Security.Principal;

namespace HiddenFoundry.ContentDeliveryApi.ExpandContentAreas;

public class ExpandContentAreaCollectionProperty : CollectionPropertyModelBase<ContentAreaItemModel, PropertyContentArea>
{
    private readonly IPrincipalAccessor _principalAccessor;
    private readonly PropertyContentArea _propertyContentArea;
    private ConverterContext _converterContext;
    private readonly Injected<IContentLoader> _contentLoader;
    private readonly Injected<EPiServer.ContentApi.Core.Serialization.Internal.ContentConvertingService> _contentConvertingService;

    public ExpandContentAreaCollectionProperty(PropertyContentArea propertyContentArea, ConverterContext converterContext) : base(propertyContentArea, converterContext)
    {
        _principalAccessor = ServiceLocator.Current.GetInstance<IPrincipalAccessor>();
        _converterContext = converterContext;
        _propertyContentArea = propertyContentArea;
        Value = GetValue();
    }

    protected override IEnumerable<ContentApiModel> ExtractExpandedValue(CultureInfo language)
    {
        var expandedModel = new List<ContentApiModel>();

        var contentAreaReferences = Value.Where(x => x.ContentLink.Id != null && x.ContentLink.WorkId != null)
            .Select(x =>
            new ContentReference
            {
                ID = x.ContentLink.Id.Value,
                ProviderName = x.ContentLink.ProviderName,
                WorkID = x.ContentLink.WorkId.Value
            });
        var contentItems = _contentLoader.Service.GetItems(contentAreaReferences, language);

        // If first time processing a content area then get the recursion value to keep tracking max iterations.
        if (_converterContext.Options is not ExtendedContentApiOptions)
        {
            ExtendedContentApiOptions newOptions = new ExtendedContentApiOptions();
            var recursiveContentAreaOptions = ServiceLocator.Current.GetInstance<IOptions<ContentAreaExpandOptions>>();
            newOptions.RecursiveLevelsRemaining = recursiveContentAreaOptions.Value.MaxExpandContentAreaLevels;

            _converterContext = GetConverterContextClone(_converterContext, newOptions);
        }

        var extendedOptions = _converterContext.Options as ExtendedContentApiOptions;
        if (extendedOptions != null && extendedOptions.RecursiveLevelsRemaining > 0)
        {
            extendedOptions.RecursiveLevelsRemaining--;

            // Create new ConverterContext to pass into next iteration. We don't want to pass a reference as we want to persist the recursiveLevelsRemaining separately along each branch

            foreach (var item in contentItems)
            {
                var contentApiModel = _contentConvertingService.Service.ConvertToContentApiModel(item, GetConverterContextClone(_converterContext, extendedOptions));
                expandedModel.Add(contentApiModel);
            }
        }

        return expandedModel;
    }

    private IEnumerable<ContentAreaItemModel> GetValue()
    {
        ContentArea contentArea = _propertyContentArea.Value as ContentArea;

        if (contentArea == null)
        {
            return null;
        }

        return (from x in FilteredItems(contentArea, ExcludePersonalizedContent)
                select new ContentAreaItemModel
                {
                    ContentLink = new ContentModelReference
                    {
                        GuidValue = x.ContentGuid,
                        Id = x.ContentLink.ID,
                        WorkId = x.ContentLink.WorkID,
                        ProviderName = x.ContentLink.ProviderName
                    },
                    DisplayOption = x.RenderSettings.ContainsKey(ContentFragment.ContentDisplayOptionAttributeName) ? x.RenderSettings[ContentFragment.ContentDisplayOptionAttributeName].ToString() : ""
                }).ToList();
    }

    private IEnumerable<ContentAreaItem> FilteredItems(ContentArea contentArea, bool excludePersonalizedContent)
    {
        if (ConverterContext.IsContentManagementRequest)
        {
            return from f in contentArea.Fragments.OfType<ContentFragment>()
                   select new ContentAreaItem(f);
        }

        // Create anon principle
        IPrincipal principal = excludePersonalizedContent ? VirtualRolePrincipal.CreateWrapper(new GenericPrincipal(new GenericIdentity("Anonymous"), new string[0])) : _principalAccessor.Principal;
        return from f in contentArea.Fragments.GetFilteredFragments(principal).OfType<ContentFragment>()
               select new ContentAreaItem(f);
    }

    private ConverterContext GetConverterContextClone(ConverterContext ConverterContext, ExtendedContentApiOptions contentApiOptions)
    {
        return new ConverterContext(contentApiOptions.Clone() as ExtendedContentApiOptions,
            string.Join(",", ConverterContext.SelectedProperties),
            string.Join(",", ConverterContext.ExpandedProperties),
            ConverterContext.ExcludePersonalizedContent,
            ConverterContext.Language);
    }

    public class ExtendedContentApiOptions : EPiServer.ContentApi.Core.Configuration.ContentApiOptions, ICloneable
    {
        public int RecursiveLevelsRemaining { get; set; }

        public object Clone()
        {
            return (ExtendedContentApiOptions)MemberwiseClone();
        }
    }
}
