using MyShop.Core.Models.Photos;

namespace MyShop.Core.Abstractions.Repositories;
public interface IPhotoRepository : IBaseReadRepository<Photo>, IBaseWriteRepository<Photo>
{
}
