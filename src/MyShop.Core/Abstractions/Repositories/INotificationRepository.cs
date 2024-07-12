using MyShop.Core.Models.Notifications;

namespace MyShop.Core.Abstractions.Repositories;
public interface INotificationRepository : IBaseReadRepository<Notification>, IBaseWriteRepository<Notification>
{
}
