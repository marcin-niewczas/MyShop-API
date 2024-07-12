using MyShop.Core.Models.Orders;

namespace MyShop.Core.Abstractions.Repositories;
public interface IOrderProductRepository : IBaseReadRepository<OrderProduct>, IBaseWriteRepository<OrderProduct>
{
}
