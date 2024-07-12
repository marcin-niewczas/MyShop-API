using MyShop.Application.Dtos.ManagementPanel.MainPageSections;
using MyShop.Application.Responses;

namespace MyShop.Application.Queries.ManagementPanel.MainPageSections;
public sealed record GetMainPageSectionMp(
    Guid Id
    ) : IQuery<ApiResponse<MainPageSectionMpDto>>;
