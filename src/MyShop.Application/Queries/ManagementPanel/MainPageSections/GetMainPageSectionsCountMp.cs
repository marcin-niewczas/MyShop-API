using MyShop.Application.Dtos;
using MyShop.Application.Responses;

namespace MyShop.Application.Queries.ManagementPanel.MainPageSections;
public sealed record GetMainPageSectionsCountMp
    : IQuery<ApiResponse<ValueDto<int>>>;
