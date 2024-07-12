using MyShop.Application.Dtos.ManagementPanel.Photos;
using MyShop.Application.EndpointQueries.Interfaces;
using MyShop.Application.Responses;
using MyShop.Application.Validations.Interfaces;
using MyShop.Application.Validations.Validators;
using MyShop.Core.Validations;

namespace MyShop.Application.Queries.ManagementPanel.Products;
public sealed record GetPagedProductPhotosMp(
    Guid Id,
    int PageNumber,
    int PageSize,
    Guid? ExpectProductVariantId
    ) : IQuery<ApiPagedResponse<PhotoMpDto>>,
        IPaginationQueryParams,
        IValidatable
{
    public void Validate(ICollection<ValidationMessage> validationMessages)
    {
        CustomValidators.PaginationParams.Validate(PageNumber, PageSize, validationMessages);
    }
}
