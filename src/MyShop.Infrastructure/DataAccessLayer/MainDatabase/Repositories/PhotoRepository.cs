using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Models.Photos;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.Repositories;
internal sealed class PhotoRepository(
    MainDbContext dbContext
    ) : BaseGenericRepository<Photo>(dbContext), 
        IPhotoRepository
{
}
