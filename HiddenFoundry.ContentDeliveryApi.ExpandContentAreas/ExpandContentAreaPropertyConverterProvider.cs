using EPiServer.ContentApi.Core.Serialization;
using EPiServer.Core;
using EPiServer.SpecializedProperties;

namespace HiddenFoundry.ContentDeliveryApi.ExpandContentAreas;

public class ExpandContentAreaPropertyConverterProvider : IPropertyConverterProvider
{
    public int SortOrder => 200;

    public IPropertyConverter Resolve(PropertyData propertyData)
    {
        if (propertyData is PropertyContentArea)
        {
            return new ExpandContentAreaPropertyConverter();
        }

        return null;
    }
}
