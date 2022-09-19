using System.ComponentModel.DataAnnotations;
using cms12_alloy.Models.Blocks;
using EPiServer.Cms.Shell.UI.ObjectEditing.EditorDescriptors;
using EPiServer.ContentApi.Core.Serialization;
using EPiServer.ContentApi.Core.Serialization.Models;
using EPiServer.PlugIn;
using EPiServer.Shell.ObjectEditing;
using EPiServer.SpecializedProperties;

namespace cms12_alloy.Models.Pages
{



    /// <summary>
    /// Used for the site's start page and also acts as a container for site settings
    /// </summary>
    [ContentType(
        GUID = "19671657-B684-4D95-A61F-8DD4FE60D559",
        GroupName = Globals.GroupNames.Specialized)]
    [SiteImageUrl]
    [AvailableContentTypes(
        Availability.Specific,
        Include = new[]
        {
        typeof(ContainerPage),
        typeof(ProductPage),
        typeof(StandardPage),
        typeof(ISearchPage),
        typeof(LandingPage),
        typeof(ContentFolder) }, // Pages we can create under the start page...
        ExcludeOn = new[]
        {
        typeof(ContainerPage),
        typeof(ProductPage),
        typeof(StandardPage),
        typeof(ISearchPage),
        typeof(LandingPage)
        })] // ...and underneath those we can't create additional start pages
    public class StartPage : SitePageData
    {
        [Display(
            GroupName = SystemTabNames.Content,
            Order = 320)]
        [CultureSpecific]
        public virtual ContentArea MainContentArea { get; set; }

        [Display(GroupName = Globals.GroupNames.SiteSettings, Order = 300)]
        public virtual LinkItemCollection ProductPageLinks { get; set; }

        [Display(GroupName = Globals.GroupNames.SiteSettings, Order = 350)]
        public virtual LinkItemCollection CompanyInformationPageLinks { get; set; }

        [Display(GroupName = Globals.GroupNames.SiteSettings, Order = 400)]
        public virtual LinkItemCollection NewsPageLinks { get; set; }

        [Display(GroupName = Globals.GroupNames.SiteSettings, Order = 450)]
        public virtual LinkItemCollection CustomerZonePageLinks { get; set; }

        [Display(GroupName = Globals.GroupNames.SiteSettings)]
        public virtual PageReference GlobalNewsPageLink { get; set; }

        [Display(GroupName = Globals.GroupNames.SiteSettings)]
        public virtual PageReference ContactsPageLink { get; set; }

        [Display(GroupName = Globals.GroupNames.SiteSettings)]
        public virtual PageReference SearchPageLink { get; set; }

        [Display(GroupName = Globals.GroupNames.SiteSettings)]
        public virtual SiteLogotypeBlock SiteLogotype { get; set; }

        [Display(GroupName = Globals.GroupNames.SiteSettings)]
        [EditorDescriptor(EditorDescriptorType = typeof(CollectionEditorDescriptor<CustomDto>))]
        public virtual IList<CustomDto> CustomDtoList3 { get; set; }
    }

    public class CustomDto
    {
        public virtual string name { get; set; }
    }

    [PropertyDefinitionTypePlugIn]
    public class CustomDtoProperty : PropertyList<CustomDto>
    {

    }

    public class CustomDtoPropertyModel : PropertyModel<IEnumerable<CustomDto>, CustomDtoProperty>
    {
        public CustomDtoPropertyModel(CustomDtoProperty type) : base(type)
        {
            Value = GetValues(type.List);
        }

        private IEnumerable<CustomDto> GetValues(IList<CustomDto> items)
        {
            return items.Select(x => new CustomDto { name = x.name });
        }
    }

    public class CustomDtoPropertyConvertor : IPropertyConverter
    {
        /// <inheritdoc />
        public IPropertyModel Convert(PropertyData propertyData, ConverterContext contentMappingContext)
        {
            return new CustomDtoPropertyModel((CustomDtoProperty)propertyData);
        }
    }
}