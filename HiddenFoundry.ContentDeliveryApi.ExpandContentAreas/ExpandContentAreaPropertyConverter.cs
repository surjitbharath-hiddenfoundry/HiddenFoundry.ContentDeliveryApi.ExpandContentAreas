using EPiServer.ContentApi.Core.Serialization;
using EPiServer.Core;
using EPiServer.SpecializedProperties;

namespace HiddenFoundry.ContentDeliveryApi.ExpandContentAreas;

public class ExpandContentAreaPropertyConverter : IPropertyConverter
{
    public IPropertyModel Convert(PropertyData propertyData, ConverterContext contentMappingContext)
    {
        var propertyModel = new ExpandContentAreaCollectionProperty(propertyData as PropertyContentArea, contentMappingContext);
        if (contentMappingContext.ShouldExpand(propertyData.Name))
        {
            propertyModel.Expand(contentMappingContext.Language);
        }

        return propertyModel;
    }
}
