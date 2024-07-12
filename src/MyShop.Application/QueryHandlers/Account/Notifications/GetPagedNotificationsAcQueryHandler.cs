using MyShop.Application.Abstractions;
using MyShop.Application.Mappings;
using MyShop.Application.Queries.Account.Notifications;
using MyShop.Application.Responses.ExtensionResponses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.RepositoryQueryParams.Account;

namespace MyShop.Application.QueryHandlers.Account.Notifications;
internal sealed class GetPagedNotificationsAcQueryHandler(
    IUserClaimsService userClaimsService,
    IUnitOfWork unitOfWork
    ) : IQueryHandler<GetPagedNotificationsAc, GetNotificationsApiPagedResponse>
{
    public async Task<GetNotificationsApiPagedResponse> HandleAsync(GetPagedNotificationsAc query, CancellationToken cancellationToken = default)
    {
        var userClaims = userClaimsService.GetUserClaimsData();

        var pagedResult = await unitOfWork.NotificationRegisteredUserRepository.GetPagedDataAsync(
            userId: userClaims.UserId,
            pageNumber: query.PageNumber,
            pageSize: query.PageSize,
            sortBy: TypeMapper.MapOptionalEnum<GetPagedNotificationsAcSortBy>(query.SortBy),
            sortDirection: TypeMapper.MapOptionalSortDirection(query.SortDirection),
            fromDate: query.FromDate,
            toDate: query.ToDate,
            inclusiveFromDate: query.InclusiveFromDate,
            inclusiveToDate: query.InclusiveToDate,
            cancellationToken: cancellationToken
            );

        int? unreadNotificationCount = null;

        if (query.WithUnreadNotificationCount == true)
        {
            unreadNotificationCount = await unitOfWork.NotificationRegisteredUserRepository.CountAsync(
                predicate: e => e.RegisteredUserId == userClaims.UserId && !e.IsRead,
                cancellationToken: cancellationToken
                );
        }

        return new(
            dtos: pagedResult.Data.ToNotificationDtos(),
            totalCount: pagedResult.TotalCount,
            pageNumber: query.PageNumber,
            pageSize: query.PageSize,
            unreadNotificationCount: unreadNotificationCount
            );
    }
}
