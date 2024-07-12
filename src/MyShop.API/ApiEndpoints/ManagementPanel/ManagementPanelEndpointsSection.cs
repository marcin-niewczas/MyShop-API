using MyShop.API.ApiEndpoints.ManagementPanel.EndpointsGroups;
using MyShop.Application.Utils;

namespace MyShop.API.ApiEndpoints.ManagementPanel;

public static class ManagementPanelEndpointsSection
{
    public static RouteGroupBuilder MapManagementPanelEndpointsSection(this RouteGroupBuilder app)
    {
        app.MapGroup("management-panel")
            .RequireAuthorization(PolicyNames.HasEmployeePermission)
            .MapManagementPanelDashboardEndpointsGroup()
            .MapManagementPanelCategoryEndpointsGroup()
            .MapManagementPanelProductOptionsEndpointsGroup()
            .MapManagementPanelProductOptionValueEndpointsGroup()
            .MapManagementPanelProductVariantEndpointsGroup()
            .MapManagementPanelProductEndpointsGroup()
            .MapManagementPanelOrderEndpointsGroup()
            .MapManagementPanelProductReviewEndpointsGroup()
            .MapManagementPanelMainPageSectionEndpointsGroup()
            .MapManagementPanelPhotoEndpointsGroup()
            .MapManagementPanelProductProductDetailOptionValueEndpointsGroup();

        return app;
    }
}
