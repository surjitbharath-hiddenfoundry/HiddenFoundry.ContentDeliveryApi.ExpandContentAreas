using cms12_alloy.Extensions;
using EPiServer.Cms.Shell;
using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.ContentApi.Core.Serialization;
using EPiServer.Scheduler;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using HiddenFoundry.ContentDeliveryApi.ExpandContentAreas;

namespace cms12_alloy;

public class Startup
{
    private readonly IWebHostEnvironment _webHostingEnvironment;

    public Startup(IWebHostEnvironment webHostingEnvironment)
    {
        _webHostingEnvironment = webHostingEnvironment;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        if (_webHostingEnvironment.IsDevelopment())
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(_webHostingEnvironment.ContentRootPath, "App_Data"));

            services.Configure<SchedulerOptions>(options => options.Enabled = false);
        }

        services
            .AddCmsAspNetIdentity<ApplicationUser>()
            .AddCms()
            .AddAlloy()
            .AddAdminUserRegistration()
            .AddEmbeddedLocalization<Startup>()
            .AddFind();

        // Required by Wangkanai.Detection
        services.AddDetection();

        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromSeconds(10);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        services.AddContentSearchApi(options =>
        {
            options.MaximumSearchResults = 10;
        });

        services.AddContentDeliveryApi(options =>
        {
            options.SiteDefinitionApiEnabled = true;
        }).WithFriendlyUrl();

        services.ConfigureContentApiOptions(o =>
        {
            o.FlattenPropertyModel = true;
            o.IncludeEmptyContentProperties = false;

        });

        services.ConfigureExpandContentAreaContentApiOptions(o =>
        {
            o.MaxExpandContentAreaLevels = 3;
        });

        //services.TryAddScoped<CustomDtoPropertyConvertor>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        // Required by Wangkanai.Detection
        app.UseDetection();
        app.UseSession();

        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseCors();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapContent();
        });
    }
}
