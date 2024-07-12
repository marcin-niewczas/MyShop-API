using MyShop.Application.Dtos.ValidatorParameters.ManagementPanel;
using MyShop.Application.Responses;

namespace MyShop.Application.Queries.ManagementPanel.MainPageSections;
public sealed record GetWebsiteHeroSectionItemValidatorParametersMp(
    Guid Id
    ) : IQuery<ApiResponse<WebsiteHeroSectionItemValidatorParametersMpDto>>;
