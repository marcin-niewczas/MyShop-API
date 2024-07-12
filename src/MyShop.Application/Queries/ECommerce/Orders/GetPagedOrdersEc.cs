using MyShop.Application.EndpointQueries.Interfaces;
using MyShop.Application.Responses;
using MyShop.Application.Validations.Interfaces;
using MyShop.Application.Validations.Validators;
using MyShop.Core.Dtos.Account;
using MyShop.Core.RepositoryQueryParams.ECommerce;
using MyShop.Core.Validations;

namespace MyShop.Application.Queries.ECommerce.Orders;
public sealed record GetPagedOrdersEc(
    int PageNumber,
    int PageSize,
    string? SortBy,
    string? SortDirection,
    DateTimeOffset? FromDate,
    DateTimeOffset? ToDate,
    bool InclusiveFromDate = true,
    bool InclusiveToDate = true,
    string? SearchPhrase = null
    ) : IQuery<ApiPagedResponse<OrderAcDto>>,
        IPaginationQueryParams,
        ISortQueryParams,
        ITimestampQueryParams,
        ISearchQueryParams,
        IValidatable
{
    public void Validate(ICollection<ValidationMessage> validationMessages)
    {
        CustomValidators.PaginationParams.Validate(PageNumber, PageSize, validationMessages);
        CustomValidators.SortParams.Validate<GetPagedOrdersEcSortBy>(SortBy, SortDirection, validationMessages);
    }
}
