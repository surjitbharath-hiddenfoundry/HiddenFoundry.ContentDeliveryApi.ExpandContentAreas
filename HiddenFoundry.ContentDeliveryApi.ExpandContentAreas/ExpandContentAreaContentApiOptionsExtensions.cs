using Microsoft.Extensions.DependencyInjection;

namespace HiddenFoundry.ContentDeliveryApi.ExpandContentAreas;
public static class ExpandContentAreaContentApiOptionsExtensions
{
    public static IServiceCollection ConfigureExpandContentAreaContentApiOptions(this IServiceCollection services, Action<ContentAreaExpandOptions> expandContentAreaOptions)
    {
        if (expandContentAreaOptions == null)
        {
            throw new ArgumentNullException("expandContentAreaOptions");
        }

        services.Configure(expandContentAreaOptions);
        return services;
    }
}

public class ContentAreaExpandOptions
{
    public int MaxExpandContentAreaLevels { get; set; } = 2;
}
