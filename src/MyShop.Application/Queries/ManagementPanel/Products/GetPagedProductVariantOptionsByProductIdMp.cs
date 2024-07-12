using MyShop.Application.Dtos.ManagementPanel.Products;
using MyShop.Application.EndpointQueries.Interfaces;
using MyShop.Application.Responses;
using MyShop.Application.Validations.Interfaces;
using MyShop.Application.Validations.Validators;
using MyShop.Core.Validations;

namespace MyShop.Application.Queries.ManagementPanel.Products;
public sealed record GetPagedProductVariantOptionsByProductIdMp(
    Guid Id,
    int PageNumber,
    int PageSize,
    string? SearchPhrase
    ) : IQuery<ApiPagedResponse<ProductVariantOptionOfProductMpDto>>,
        IPaginationQueryParams,
        ISearchQueryParams,
        IValidatable
{
    public void Validate(ICollection<ValidationMessage> validationMessages)
    {
        CustomValidators.PaginationParams.Validate(PageNumber, PageSize, validationMessages);
    }
}
