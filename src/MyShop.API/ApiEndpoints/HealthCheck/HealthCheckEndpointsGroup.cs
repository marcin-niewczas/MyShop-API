namespace MyShop.API.ApiEndpoints.HealthCheck;

public static class HealthCheckEndpointsGroup
{
    public static RouteGroupBuilder MapHealthCheckEndpointsGroup(this RouteGroupBuilder app)
    {
        app.MapGroup("/health-checks")
           .WithTags("HealthChecks")
           .MapHealthCheckEndpoints();

        return app;
    }

    private static RouteGroupBuilder MapHealthCheckEndpoints(this RouteGroupBuilder app)
    {
        app.MapHealthChecks("/");

        return app;
    }
}
