using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Models.Users;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.Repositories;
internal sealed class RegisteredUserRespository(
    MainDbContext dbContext
    ) : BaseGenericRepository<RegisteredUser>(dbContext), 
        IRegisteredUserRepository
{
}
