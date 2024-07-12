using MyShop.Application.EndpointQueries.Interfaces;
using MyShop.Application.EndpointQueries.ProductOptions;
using MyShop.Application.Responses;
using MyShop.Application.Validations.Interfaces;
using MyShop.Application.Validations.Validators;
using MyShop.Core.Dtos.Shared;
using MyShop.Core.RepositoryQueryParams.ECommerce;
using MyShop.Core.Validations;

namespace MyShop.Application.Queries.ECommerce.Products;
public sealed record GetPagedProductsEc(
    int PageNumber,
    int PageSize,
    string SortBy,
    string? EncodedCategoryName,
    ProductOptionParam? ProductOptionParam,
    decimal? MinPrice,
    decimal? MaxPrice,
    string? SearchPhrase
    ) : IQuery<ApiPagedResponse<ProductItemDto>>, IPaginationQueryParams, ISearchQueryParams, IValidatable
{
    public void Validate(ICollection<ValidationMessage> validationMessages)
    {
        CustomValidators.PaginationParams.Validate(PageNumber, PageSize, validationMessages);
        CustomValidators.Enums.IsInEnum<GetPagedProductsEcSortBy>(SortBy, validationMessages, nameof(SortBy));
    }
}
