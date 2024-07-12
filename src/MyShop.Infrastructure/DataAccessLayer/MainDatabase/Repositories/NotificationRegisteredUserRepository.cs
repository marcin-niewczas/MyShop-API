using Microsoft.EntityFrameworkCore;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.HelperModels;
using MyShop.Core.Models.Notifications;
using MyShop.Core.RepositoryQueryParams.Account;
using MyShop.Core.RepositoryQueryParams.Commons;
using MyShop.Infrastructure.DataAccessLayer.MainDatabase.Repositories.Utils;
using System.Linq.Expressions;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.Repositories;
internal sealed class NotificationRegisteredUserRepository(
    MainDbContext dbContext
    ) : BaseGenericRepository<NotificationRegisteredUser>(dbContext),
        INotificationRegisteredUserRepository
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
        )
    {
        var baseQuery = _dbSet
            .Include(i => i.Notification)
            .Where(e => e.RegisteredUserId == userId);

        if (fromDate is not null)
        {
            baseQuery = inclusiveFromDate switch
            {
                false => baseQuery.Where(e => fromDate == null || e.CreatedAt > fromDate),
                _ => baseQuery.Where(e => fromDate == null || e.CreatedAt >= fromDate),
            };
        }

        if (toDate is not null)
        {
            toDate = toDate.Value.AddDays(1);

            baseQuery = inclusiveToDate switch
            {
                false => baseQuery.Where(e => toDate == null || e.CreatedAt < toDate),
                _ => baseQuery.Where(e => toDate == null || e.CreatedAt <= toDate),
            };
        }

        Expression<Func<NotificationRegisteredUser, object?>> sortByExpression = sortBy switch
        {
            GetPagedNotificationsAcSortBy.NotificationType => x => x.Notification.NotificationType,
            GetPagedNotificationsAcSortBy.IsRead => x => x.IsRead,
            GetPagedNotificationsAcSortBy.UpdatedAt => x => x.UpdatedAt,
            _ => x => x.CreatedAt
        };

        baseQuery = sortDirection switch
        {
            SortDirection.Ascendant => baseQuery.OrderBy(sortByExpression),
            _ => baseQuery.OrderByDescending(sortByExpression)
        };

        return baseQuery.ToPagedResultAsync(
            pageNumber,
            pageSize,
            cancellationToken: cancellationToken
            );
    }
}
