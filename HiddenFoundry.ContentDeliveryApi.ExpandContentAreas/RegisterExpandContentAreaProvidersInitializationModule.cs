using EPiServer.ContentApi.Core.Serialization;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using Microsoft.Extensions.DependencyInjection;

namespace HiddenFoundry.ContentDeliveryApi.ExpandContentAreas
{
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class RegisterExpandContentAreaProvidersInitializationModule : IConfigurableModule
    {
        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            var services = context.Services;
            services.AddSingleton<IPropertyConverterProvider, ExpandContentAreaPropertyConverterProvider>();
            services.AddSingleton<IPropertyConverter, ExpandContentAreaPropertyConverter>();
        }

        public void Initialize(InitializationEngine context)
        {
        }

        public void Uninitialize(InitializationEngine context)
        {
        }
    }
}