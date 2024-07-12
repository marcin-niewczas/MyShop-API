using MyShop.Application.Dtos.ManagementPanel.Photos;
using MyShop.Application.Responses;
using MyShop.Application.Validations.Interfaces;
using MyShop.Application.Validations.Validators;
using MyShop.Core.Validations;

namespace MyShop.Application.Queries.ManagementPanel.Photos;
public sealed record GetPagedWebsiteHeroSectionPhotosMp(
    int PageNumber,
    int PageSize
    ) : IQuery<ApiPagedResponse<PhotoMpDto>>, IValidatable
{
    public void Validate(ICollection<ValidationMessage> validationMessages)
    {
        CustomValidators.PaginationParams.Validate(PageNumber, PageSize, validationMessages);
    }
}
