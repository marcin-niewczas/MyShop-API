using MyShop.Core.HelperModels;
using MyShop.Core.Models.Notifications;
using MyShop.Core.RepositoryQueryParams.Account;
using MyShop.Core.RepositoryQueryParams.Commons;

namespace MyShop.Core.Abstractions.Repositories;
public interface INotificationRegisteredUserRepository : IBaseReadRepository<NotificationRegisteredUser>, IBaseWriteRepository<NotificationRegisteredUser>
{
    public Task<PagedResult<NotificationRegisteredUser>> GetPagedDataAsync(
        Guid userId,
        int pageNumber,
        int pageSize,
        GetPagedNotificationsAcSortBy? sortBy,
        SortDirection? sortDirection,
        DateTimeOffset? fromDate,
        DateTimeOffset? toDate,
        bool? inclusiveFromDate,
        bool? inclusiveToDate,
        CancellationToken cancellationToken = default
        );
}
