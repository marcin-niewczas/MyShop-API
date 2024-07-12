using MyShop.Core.Models.Users;

namespace MyShop.Core.Abstractions.Repositories;
public interface IUserAddressRepository : IBaseReadRepository<UserAddress>, IBaseWriteRepository<UserAddress>
{
}
