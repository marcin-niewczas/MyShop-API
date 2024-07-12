using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Models.Orders;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.Repositories;
internal sealed class OrderProductRepository(
    MainDbContext dbContext
    ) : BaseGenericRepository<OrderProduct>(dbContext), IOrderProductRepository
{
}
