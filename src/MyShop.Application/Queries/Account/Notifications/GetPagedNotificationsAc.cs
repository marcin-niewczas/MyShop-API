using MyShop.Application.EndpointQueries.Interfaces;
using MyShop.Application.Responses.ExtensionResponses;
using MyShop.Application.Validations.Interfaces;
using MyShop.Application.Validations.Validators;
using MyShop.Core.RepositoryQueryParams.Account;
using MyShop.Core.Validations;

namespace MyShop.Application.Queries.Account.Notifications;
public sealed record GetPagedNotificationsAc : IQuery<GetNotificationsApiPagedResponse>, IValidatable, IPaginationQueryParams, ISortQueryParams
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public string? SortDirection { get; init; }
    public string? SortBy { get; init; }
    public DateTimeOffset? FromDate { get; init; }
    public DateTimeOffset? ToDate { get; init; }
    public bool? InclusiveFromDate { get; init; }
    public bool? InclusiveToDate { get; init; }
    public bool? WithUnreadNotificationCount { get; init; }

    public void Validate(ICollection<ValidationMessage> validationMessages)
    {
        CustomValidators.PaginationParams.Validate(PageNumber, PageSize, validationMessages);
        CustomValidators.SortParams.Validate<GetPagedNotificationsAcSortBy>(SortBy, SortDirection, validationMessages);
    }
}
