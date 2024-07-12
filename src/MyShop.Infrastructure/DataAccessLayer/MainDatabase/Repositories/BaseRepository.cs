namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.Repositories;
internal abstract class BaseRepository(
    MainDbContext dbContext
    )
{
    protected readonly MainDbContext _dbContext = dbContext;
}
