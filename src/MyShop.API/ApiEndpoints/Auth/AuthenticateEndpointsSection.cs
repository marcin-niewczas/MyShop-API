using MyShop.API.ApiEndpoints.Auth.EndpointsGroups;

namespace MyShop.API.ApiEndpoints.Auth;

public static class AuthenticateEndpointsSection
{
    public static RouteGroupBuilder MapAuthenticateEndpointsSection(this RouteGroupBuilder app)
    {
        app.MapGroup("/authenticates")
           .MapAuthenticateEndpointsGroup();

        return app;
    }
}
