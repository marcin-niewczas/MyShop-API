using MyShop.Application.EndpointQueries.Interfaces;
using MyShop.Application.Responses;
using MyShop.Application.Validations.Interfaces;
using MyShop.Application.Validations.Validators;
using MyShop.Core.Dtos.ManagementPanel;
using MyShop.Core.RepositoryQueryParams.ManagementPanel;
using MyShop.Core.Validations;

namespace MyShop.Application.Queries.ManagementPanel.Orders;
public sealed record GetPagedOrdersMp(
    int PageNumber,
    int PageSize,
    string? SortBy,
    string? SortDirection,
    DateTimeOffset? FromDate,
    DateTimeOffset? ToDate,
    bool InclusiveFromDate = true,
    bool InclusiveToDate = true,
    string? SearchPhrase = null
    ) : IQuery<ApiPagedResponse<PagedOrderMpDto>>,
        IPaginationQueryParams,
        ISortQueryParams,
        ITimestampQueryParams,
        ISearchQueryParams,
        IValidatable
{
    public void Validate(ICollection<ValidationMessage> validationMessages)
    {
        CustomValidators.PaginationParams.Validate(PageNumber, PageSize, validationMessages);
        CustomValidators.SortParams.Validate<GetPagedOrdersMpSortBy>(SortBy, SortDirection, validationMessages);
    }
}
