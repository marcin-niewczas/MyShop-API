using MyShop.Application.Dtos.ManagementPanel.MainPageSections;
using MyShop.Application.Responses;
using MyShop.Application.Validations.Interfaces;
using MyShop.Application.Validations.Validators;
using MyShop.Core.Validations;

namespace MyShop.Application.Queries.ManagementPanel.MainPageSections;
public sealed record GetPagedWebsiteHeroSectionItemsMp(
    Guid Id,
    int PageNumber,
    int PageSize,
    bool Active
    ) : IQuery<ApiPagedResponse<WebsiteHeroSectionItemMpDto>>, IValidatable
{
    public void Validate(ICollection<ValidationMessage> validationMessages)
    {
        CustomValidators.PaginationParams.Validate(PageNumber, PageSize, validationMessages);
    }
}
