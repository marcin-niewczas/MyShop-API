using MyShop.Core.Models.BaseEntities;

namespace MyShop.Core.Abstractions.Repositories;
public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    IBaseProductOptionRepository BaseProductOptionRepository { get; }
    IBaseProductOptionValueRepository BaseProductOptionValueRepository { get; }
    IDashboardRepository DashboardRepository { get; }
    ICategoryRepository CategoryRepository { get; }
    IFavoriteRepository FavoriteRepository { get; }
    IInvoiceRepository InvoiceRepository { get; }
    IMainPageSectionRepository MainPageSectionRepository { get; }
    INotificationRepository NotificationRepository { get; }
    INotificationRegisteredUserRepository NotificationRegisteredUserRepository { get; }
    IOrderRepository OrderRepository { get; }
    IOrderProductRepository OrderProductRepository { get; }
    IPhotoRepository PhotoRepository { get; }
    IProductVariantPhotoRepository ProductVariantPhotoRepository { get; }
    IProductRepository ProductRepository { get; }
    IProductDetailOptionRepository ProductDetailOptionRepository { get; }
    IProductDetailOptionValueRepository ProductDetailOptionValueRepository { get; }
    IProductProductDetailOptionValueRepository ProductProductDetailOptionValueRespository { get; }
    IProductProductVariantOptionRepository ProductProductVariantOptionRespository { get; }
    IProductReviewRepository ProductReviewRepository { get; }
    IProductVariantRepository ProductVariantRepository { get; }
    IProductVariantOptionRepository ProductVariantOptionRepository { get; }
    IProductVariantOptionValueRepository ProductVariantOptionValueRepository { get; }
    IProductVariantPhotoItemRepository ProductVariantPhotoItemRepository { get; }
    IRegisteredUserRepository RegisteredUserRepository { get; }
    IShoppingCartRepository ShoppingCartRepository { get; }
    IShoppingCartItemRepository ShoppingCartItemRepository { get; }
    IUserRepository UserRepository { get; }
    IUserAddressRepository UserAddressRepository { get; }
    IUserTokenRepository UserTokenRepository { get; }
    IWebsiteHeroSectionRepository WebsiteHeroSectionRepository { get; }
    IWebsiteHeroSectionItemRepository WebsiteHeroSectionItemRepository { get; }
    IWebsiteHeroSectionPhotoRepository WebsiteHeroSectionPhotoRepository { get; }
    IWebsiteProductCarouselSectionRepository WebsiteProductCarouselSectionRepository { get; }

    void Attach<TEntity>(TEntity entity) where TEntity : class, IEntity;
    void AttachRange<TEntity>(IEnumerable<TEntity> entites) where TEntity : class, IEntity;
    Task<TEntity> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class, IEntity;
    Task AddRangeAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) where TEntity : class, IEntity;
    Task<TEntity> UpdateAsync<TEntity>(TEntity entity) where TEntity : class, IEntity;
    Task UpdateAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, IEntity;
    Task<TEntity> RemoveAsync<TEntity>(TEntity entity) where TEntity : class, IEntity;
    void DetectChanges();
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
