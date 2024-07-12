using MyShop.API.ApiEndpoints.ECommerce.EndpointsGroups;

namespace MyShop.API.ApiEndpoints.ECommerce;

public static class ECommerceEndpointsSection
{
    public static RouteGroupBuilder MapECommerceEndpointsSection(this RouteGroupBuilder app)
    {
        app.MapGroup("/e-commerce")
            .MapEcommerceShoppingCartEndpointsGroup()
            .MapEcommerceCategoryEndpointsGroup()
            .MapEcommerceOrderEndpointsGroup()
            .MapEcommerceProductReviewEndpointsGroup()
            .MapEcommerceProductEndpointsGroup()
            .MapEcommerceFavoriteEndpointsGroup()
            .MapEcommerceMainPageSectionEndpointsGroup();

        return app;
    }
}
