using MyShop.API.ApiEndpoints.Account.EndpointsGroups;
using MyShop.Application.Utils;

namespace MyShop.API.ApiEndpoints.Account;

public static class AccountEndpointsSection
{
    public static RouteGroupBuilder MapAccountEndpointsSection(this RouteGroupBuilder app)
    {
        app.MapGroup("/accounts")
            .RequireAuthorization(PolicyNames.HasCustomerPermission)
            .MapAccountFavoriteEndpointsGroup()
            .MapAccountUserEndpointsGroup()
            .MapAccountNotficicationEndpointsGroup();

        return app;
    }
}
