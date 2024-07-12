using MyShop.API.ApiEndpoints.Account;
using MyShop.API.ApiEndpoints.Auth;
using MyShop.API.ApiEndpoints.Configuration;
using MyShop.API.ApiEndpoints.ECommerce;
using MyShop.API.ApiEndpoints.HealthCheck;
using MyShop.API.ApiEndpoints.ManagementPanel;

namespace MyShop.API.ApiEndpoints;

public static class Extensions
{
    public static WebApplication MapApiEndpoints(this WebApplication app)
    {
        app.MapGroup("/api/v1")
           .MapAccountEndpointsSection()
           .MapAuthenticateEndpointsSection()
           .MapConfigurationEndpointsGroup()
           .MapECommerceEndpointsSection()
           .MapHealthCheckEndpointsGroup()
           .MapManagementPanelEndpointsSection();

        return app;
    }
}
