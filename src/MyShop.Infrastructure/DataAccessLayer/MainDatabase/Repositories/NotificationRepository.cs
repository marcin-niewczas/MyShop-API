using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Models.Notifications;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.Repositories;
internal sealed class NotificationRepository(
    MainDbContext dbContext
    ) : BaseGenericRepository<Notification>(dbContext), INotificationRepository
{
}
