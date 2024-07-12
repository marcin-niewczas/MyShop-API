using MyShop.Application.Dtos.ManagementPanel.MainPageSections;
using MyShop.Application.Responses;

namespace MyShop.Application.Queries.ManagementPanel.MainPageSections;
public sealed record GetWebsiteHeroSectionItemMp(
    Guid Id
    ) : IQuery<ApiResponse<WebsiteHeroSectionItemMpDto>>;
