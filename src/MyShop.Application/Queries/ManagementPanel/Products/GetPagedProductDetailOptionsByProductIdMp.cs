using MyShop.Application.Dtos.ManagementPanel.Products;
using MyShop.Application.EndpointQueries.Interfaces;
using MyShop.Application.Responses;
using MyShop.Application.Validations.Interfaces;
using MyShop.Application.Validations.Validators;
using MyShop.Core.Validations;

namespace MyShop.Application.Queries.ManagementPanel.Products;
public sealed record GetPagedProductDetailOptionsByProductIdMp(
    Guid Id,
    int PageNumber,
    int PageSize
    ) : IQuery<ApiPagedResponse<ProductDetailOptionOfProductMpDto>>,
        IPaginationQueryParams,
        IValidatable
{
    public void Validate(ICollection<ValidationMessage> validationMessages)
    {
        CustomValidators.PaginationParams.Validate(PageNumber, PageSize, validationMessages);
    }
}
