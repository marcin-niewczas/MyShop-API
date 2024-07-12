using MyShop.Application.EndpointQueries.Interfaces;
using MyShop.Application.Responses;
using MyShop.Application.Validations.Interfaces;
using MyShop.Application.Validations.Validators;
using MyShop.Core.Dtos.ManagementPanel;
using MyShop.Core.RepositoryQueryParams.ManagementPanel;
using MyShop.Core.Validations;

namespace MyShop.Application.Queries.ManagementPanel.Products;
public sealed record GetPagedProductsMp(
    int PageNumber,
    int PageSize,
    string? SortBy,
    string? SortDirection,
    string? SearchPhrase
    ) : IQuery<ApiPagedResponse<PagedProductMpDto>>,
        IPaginationQueryParams,
        ISortQueryParams,
        ISearchQueryParams,
        IValidatable
{
    public void Validate(ICollection<ValidationMessage> validationMessages)
    {
        CustomValidators.PaginationParams.Validate(PageNumber, PageSize, validationMessages);
        CustomValidators.SortParams.Validate<GetPagedProductsMpSortBy>(SortBy, SortDirection, validationMessages);
    }
}
