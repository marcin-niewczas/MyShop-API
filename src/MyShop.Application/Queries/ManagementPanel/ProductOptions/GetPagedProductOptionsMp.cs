using MyShop.Application.Dtos.ManagementPanel.ProductOptions;
using MyShop.Application.EndpointQueries.Interfaces;
using MyShop.Application.Responses;
using MyShop.Application.Validations.Interfaces;
using MyShop.Application.Validations.Validators;
using MyShop.Core.RepositoryQueryParams.ManagementPanel;
using MyShop.Core.Validations;

namespace MyShop.Application.Queries.ManagementPanel.ProductOptions;
public sealed record GetPagedProductOptionsMp(
    int PageNumber,
    int PageSize,
    string? SortBy,
    string? SortDirection,
    string? SearchPhrase,
    string QueryType,
    string SubqueryType
    ) : IQuery<ApiPagedResponse<ProductOptionMpDto>>,
        IPaginationQueryParams,
        ISortQueryParams,
        ISearchQueryParams,
        IQueryTypeParams,
        ISubqueryType,
        IValidatable
{
    public void Validate(ICollection<ValidationMessage> validationMessages)
    {
        CustomValidators.PaginationParams.Validate(PageNumber, PageSize, validationMessages);
        CustomValidators.SortParams.Validate<GetPagedProductOptionsMpSortBy>(SortBy, SortDirection, validationMessages);
        CustomValidators.Enums.IsInEnum<ProductOptionTypeMpQueryType>(QueryType, validationMessages, nameof(QueryType));
        CustomValidators.Enums.IsInEnum<ProductOptionSubtypeMpQueryType>(SubqueryType, validationMessages, nameof(SubqueryType));
    }
}
