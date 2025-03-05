using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Models.BaseEntities;
using MyShop.Infrastructure.DataAccessLayer.MainDatabase.Repositories;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase;
internal sealed class UnitOfWork(
    MainDbContext dbContext
    ) : IUnitOfWork
{
    public IBaseProductOptionRepository BaseProductOptionRepository => _productOptionRepository ??= new BaseProductOptionRepository(dbContext);
    public IBaseProductOptionValueRepository BaseProductOptionValueRepository => _productOptionValueRepository ??= new BaseProductOptionValueRepository(dbContext);
    public ICategoryRepository CategoryRepository => _categoryRepository ??= new CategoryRepository(dbContext);
    public IDashboardRepository DashboardRepository => _dashboardRepository ??= new DashboardRepository(dbContext);
    public IFavoriteRepository FavoriteRepository => _favoriteRepository ??= new FavoriteRepository(dbContext);
    public IInvoiceRepository InvoiceRepository => _invoiceRepository ??= new InvoiceRepository(dbContext);
    public IMainPageSectionRepository MainPageSectionRepository => _mainPageSectionRepository ??= new MainPageSectionRepository(dbContext);
    public INotificationRepository NotificationRepository => _notificationRepository ??= new NotificationRepository(dbContext);
    public INotificationRegisteredUserRepository NotificationRegisteredUserRepository => _notificationRegisteredUserRepository ??= new NotificationRegisteredUserRepository(dbContext);
    public IOrderRepository OrderRepository => _orderRepository ??= new OrderRepository(dbContext);
    public IOrderProductRepository OrderProductRepository => _orderProductRepository ??= new OrderProductRepository(dbContext);
    public IPhotoRepository PhotoRepository => _photoRepository ??= new PhotoRepository(dbContext);
    public IProductVariantPhotoRepository ProductVariantPhotoRepository => _productVariantPhotoRepository ??= new ProductVariantPhotoRepository(dbContext);
    public IProductRepository ProductRepository => _productRepository ??= new ProductRepository(dbContext);
    public IProductDetailOptionRepository ProductDetailOptionRepository => _productDetailOptionRepository ??= new ProductDetailOptionRepository(dbContext);
    public IProductDetailOptionValueRepository ProductDetailOptionValueRepository => _productDetailOptionValueRepository ??= new ProductDetailOptionValueRepository(dbContext);
    public IProductProductDetailOptionValueRepository ProductProductDetailOptionValueRespository => _productProductDetailOptionValueRepository ??= new ProductProductDetailOptionValueRepository(dbContext);
    public IProductProductVariantOptionRepository ProductProductVariantOptionRespository => _productProductVariantOptionRepository ??= new ProductProductVariantOptionRepository(dbContext);
    public IProductReviewRepository ProductReviewRepository => _productReviewRepository ??= new ProductReviewRespository(dbContext);
    public IProductVariantRepository ProductVariantRepository => _productVariantRepository ??= new ProductVariantRepository(dbContext);
    public IProductVariantOptionRepository ProductVariantOptionRepository => _productVariantOptionRepository ??= new ProductVariantOptionRepository(dbContext);
    public IProductVariantOptionValueRepository ProductVariantOptionValueRepository => _productVariantOptionValueRepository ??= new ProductVariantOptionValueRepository(dbContext);
    public IProductVariantPhotoItemRepository ProductVariantPhotoItemRepository => _productVariantPhotoItemRepository ??= new ProductVariantPhotoItemRepository(dbContext);
    public IRegisteredUserRepository RegisteredUserRepository => _registeredUserRepository ??= new RegisteredUserRespository(dbContext);
    public IShoppingCartRepository ShoppingCartRepository => _shoppingCartRepository ??= new ShoppingCartRepository(dbContext);
    public IShoppingCartItemRepository ShoppingCartItemRepository => _shoppingCartItemRepository ??= new ShoppingCartItemRepository(dbContext);
    public IUserRepository UserRepository => _userRepository ??= new UserRepository(dbContext);
    public IUserAddressRepository UserAddressRepository => _userAddressRepository ??= new UserAddressRepository(dbContext);
    public IUserTokenRepository UserTokenRepository => _userTokenRepository ??= new UserTokenRepository(dbContext);
    public IWebsiteHeroSectionRepository WebsiteHeroSectionRepository => _websiteHeroSectionRepository ??= new WebsiteHeroSectionRepository(dbContext);
    public IWebsiteHeroSectionItemRepository WebsiteHeroSectionItemRepository => _websiteHeroSectionItemRepository ??= new WebsiteHeroSectionItemRepository(dbContext);
    public IWebsiteHeroSectionPhotoRepository WebsiteHeroSectionPhotoRepository => _websiteHeroSectionPhotoRepository ??= new WebsiteHeroSectionPhotoRepository(dbContext);
    public IWebsiteProductCarouselSectionRepository WebsiteProductCarouselSectionRepository => _websiteProductCarouselSectionRepository ??= new WebsiteProductCarouselSectionRepository(dbContext);

    private IBaseProductOptionRepository _productOptionRepository = default!;
    private IBaseProductOptionValueRepository _productOptionValueRepository = default!;
    private ICategoryRepository _categoryRepository = default!;
    private IDashboardRepository _dashboardRepository = default!;
    private IFavoriteRepository _favoriteRepository = default!;
    private IInvoiceRepository _invoiceRepository = default!;
    private IMainPageSectionRepository _mainPageSectionRepository = default!;
    private INotificationRepository _notificationRepository = default!;
    private INotificationRegisteredUserRepository _notificationRegisteredUserRepository = default!;
    private IOrderRepository _orderRepository = default!;
    private IOrderProductRepository _orderProductRepository = default!;
    private IPhotoRepository _photoRepository = default!;
    private IProductVariantPhotoRepository _productVariantPhotoRepository = default!;
    private IProductRepository _productRepository = default!;
    private IProductDetailOptionRepository _productDetailOptionRepository = default!;
    private IProductDetailOptionValueRepository _productDetailOptionValueRepository = default!;
    private IProductProductDetailOptionValueRepository _productProductDetailOptionValueRepository = default!;
    private IProductProductVariantOptionRepository _productProductVariantOptionRepository = default!;
    private IProductReviewRepository _productReviewRepository = default!;
    private IProductVariantRepository _productVariantRepository = default!;
    private IProductVariantOptionRepository _productVariantOptionRepository = default!;
    private IProductVariantOptionValueRepository _productVariantOptionValueRepository = default!;
    private IProductVariantPhotoItemRepository _productVariantPhotoItemRepository = default!;
    private IRegisteredUserRepository _registeredUserRepository = default!;
    private IShoppingCartRepository _shoppingCartRepository = default!;
    private IShoppingCartItemRepository _shoppingCartItemRepository = default!;
    private IUserRepository _userRepository = default!;
    private IUserAddressRepository _userAddressRepository = default!;
    private IUserTokenRepository _userTokenRepository = default!;
    private IWebsiteHeroSectionRepository _websiteHeroSectionRepository = default!;
    private IWebsiteHeroSectionItemRepository _websiteHeroSectionItemRepository = default!;
    private IWebsiteHeroSectionPhotoRepository _websiteHeroSectionPhotoRepository = default!;
    private IWebsiteProductCarouselSectionRepository _websiteProductCarouselSectionRepository = default!;

    private bool _disposed = false;

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => dbContext.SaveChangesAsync(cancellationToken);

    public async Task<TEntity> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class, IEntity
        => (await dbContext.AddAsync(entity, cancellationToken)).Entity;

    public Task AddRangeAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) where TEntity : class, IEntity
        => dbContext.AddRangeAsync(entities, cancellationToken);

    public Task<TEntity> UpdateAsync<TEntity>(TEntity entity) where TEntity : class, IEntity
    {
        dbContext.Update(entity);
        return Task.FromResult(entity);
    }

    public Task UpdateAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, IEntity
    {
        dbContext.UpdateRange(entities);
        return Task.CompletedTask;
    }

    public Task<TEntity> RemoveAsync<TEntity>(TEntity entity) where TEntity : class, IEntity
    {
        dbContext.Remove(entity);
        return Task.FromResult(entity);
    }

    public void Attach<TEntity>(TEntity entity) where TEntity : class, IEntity
        => dbContext.Attach(entity);

    public void AttachRange<TEntity>(IEnumerable<TEntity> entites) where TEntity : class, IEntity
        => dbContext.AttachRange(entites);

    public void DetectChanges()
        => dbContext.ChangeTracker.DetectChanges();

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsync(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                dbContext.Dispose();
            }
        }

        _disposed = true;
    }

    private async ValueTask DisposeAsync(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                await dbContext.DisposeAsync();
            }
        }

        _disposed = true;
    }
}
