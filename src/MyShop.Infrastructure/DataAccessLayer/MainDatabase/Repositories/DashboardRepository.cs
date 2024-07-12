using Microsoft.EntityFrameworkCore;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.HelperModels;
using MyShop.Core.Models.Products;
using MyShop.Core.ValueObjects.Categories;
using MyShop.Core.ValueObjects.Orders;
using MyShop.Core.ValueObjects.ProductOptions;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.Repositories;
internal sealed class DashboardRepository(
    MainDbContext dbContext
    ) : BaseRepository(dbContext), IDashboardRepository
{
    public Task<decimal> GetTotalSalesAsync(CancellationToken cancellationToken = default)
        => _dbContext
            .OrderProducts
            .Where(e => e.Order.Status == OrderStatus.PaymentReceived || e.Order.Status == OrderStatus.Completed)
            .SumAsync(e => e.Quantity * e.Price, cancellationToken);

    public Task<int> GetTotalProductVariantsCountAsync(CancellationToken cancellationToken = default)
        => _dbContext
            .ProductVariants
            .CountAsync(cancellationToken);

    public Task<int> GetTotalCategoriesCountAsync(int maxCategoryLevel = CategoryLevel.Max, CancellationToken cancellationToken = default)
        => _dbContext
            .Categories
            .Where(e => e.HierarchyDetail.Level == maxCategoryLevel)
            .CountAsync(cancellationToken);

    public Task<int> GetTotalCustomersAsync(CancellationToken cancellationToken = default)
        => _dbContext
            .Customers
            .CountAsync(cancellationToken);

    public Task<int> GetTotalOrdersAsync(CancellationToken cancellationToken = default)
        => _dbContext
            .Orders
            .CountAsync(cancellationToken);

    public async Task<IReadOnlyCollection<ChartData<DateTimeOffset>>> GetYearSalesPerMonthAsync(DateTimeOffset? now = null, CancellationToken cancellationToken = default)
    {
        now ??= DateTimeOffset.Now;

        now = now.Value
            .AddMonths(1)
            .AddYears(-1);

        var queryDate = new DateTimeOffset(now.Value.Year, now.Value.Month, 1, 0, 0, 0, now.Value.Offset);

        var result = await _dbContext
           .Orders
           .Where(e => e.CreatedAt >= queryDate)
           .Where(e => e.Status == OrderStatus.PaymentReceived || e.Status == OrderStatus.Completed)
           .GroupBy(e => e.CreatedAt.Month)
           .Select(e => new
           {
               Label = e.First().CreatedAt,
               Value = e.Select(o => o.OrderProducts.Select(p => p.Price * p.Quantity))
               .SelectMany(e => e)
               .Sum()
           })
           .OrderBy(e => e.Label)
           .Select(e => new ChartData<DateTimeOffset>
           {
               Label = e.Label,
               Value = e.Value
           })
           .ToListAsync(cancellationToken);

        return result;
    }

    public Task<decimal> GetTodaySalesAsync(DateTimeOffset? now = null, CancellationToken cancellationToken = default)
    {
        now ??= DateTimeOffset.Now;

        var queryDate = new DateTimeOffset(now.Value.Year, now.Value.Month, now.Value.Day, 0, 0, 0, now.Value.Offset);

        return _dbContext
          .OrderProducts
          .Where(e => e.Order.CreatedAt >= queryDate)
          .Where(e => e.Order.Status == OrderStatus.PaymentReceived || e.Order.Status == OrderStatus.Completed)
          .SumAsync(e => e.Price * e.Quantity, cancellationToken);
    }

    public Task<int> GetTodayOrdersCountAsync(DateTimeOffset? now = null, CancellationToken cancellationToken = default)
    {
        now ??= DateTimeOffset.Now;

        var queryDate = new DateTimeOffset(now.Value.Year, now.Value.Month, now.Value.Day, 0, 0, 0, now.Value.Offset);

        return _dbContext
          .Orders
          .Where(e => e.CreatedAt >= queryDate)
          .CountAsync(cancellationToken);
    }

    public Task<decimal> GetThisWeekSalesAsync(DateTimeOffset? now = null, CancellationToken cancellationToken = default)
    {
        now ??= DateTimeOffset.Now;

        var dayToSubstract = ((int)now.Value.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7;
        var queryDate = new DateTimeOffset(now.Value.Year, now.Value.Month, now.Value.Day, 0, 0, 0, now.Value.Offset).AddDays(-dayToSubstract);

        return _dbContext
          .OrderProducts
          .Where(e => e.Order.CreatedAt >= queryDate)
          .Where(e => e.Order.Status == OrderStatus.PaymentReceived || e.Order.Status == OrderStatus.Completed)
          .SumAsync(e => e.Price * e.Quantity, cancellationToken);
    }

    public Task<int> GetThisWeekOrdersCountAsync(DateTimeOffset? now = null, CancellationToken cancellationToken = default)
    {
        now ??= DateTimeOffset.Now;

        var dayToSubstract = ((int)now.Value.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7;
        var queryDate = new DateTimeOffset(now.Value.Year, now.Value.Month, now.Value.Day, 0, 0, 0, now.Value.Offset).AddDays(-dayToSubstract);

        return _dbContext
          .Orders
          .Where(e => e.CreatedAt >= queryDate)
          .CountAsync(cancellationToken);
    }

    public Task<decimal> GetThisMonthSalesAsync(DateTimeOffset? now = null, CancellationToken cancellationToken = default)
    {
        now ??= DateTimeOffset.Now;

        var queryDate = new DateTimeOffset(now.Value.Year, now.Value.Month, 1, 0, 0, 0, now.Value.Offset);

        return _dbContext
          .OrderProducts
          .Where(e => e.Order.CreatedAt >= queryDate)
          .Where(e => e.Order.Status == OrderStatus.PaymentReceived || e.Order.Status == OrderStatus.Completed)
          .SumAsync(e => e.Price * e.Quantity, cancellationToken);
    }

    public Task<int> GetThisMonthOrdersCountAsync(DateTimeOffset? now = null, CancellationToken cancellationToken = default)
    {
        now ??= DateTimeOffset.Now;

        var queryDate = new DateTimeOffset(now.Value.Year, now.Value.Month, 1, 0, 0, 0, now.Value.Offset);

        return _dbContext
          .Orders
          .Where(e => e.CreatedAt >= queryDate)
          .CountAsync(cancellationToken);
    }

    public Task<decimal> GetThisYearSalesAsync(DateTimeOffset? now = null, CancellationToken cancellationToken = default)
    {
        now ??= DateTimeOffset.Now;

        var queryDate = new DateTimeOffset(now.Value.Year, 1, 1, 0, 0, 0, now.Value.Offset);

        return _dbContext
          .OrderProducts
          .Where(e => e.Order.CreatedAt >= queryDate)
          .Where(e => e.Order.Status == OrderStatus.PaymentReceived || e.Order.Status == OrderStatus.Completed)
          .SumAsync(e => e.Price * e.Quantity, cancellationToken);
    }

    public Task<int> GetThisYearOrdersCountAsync(DateTimeOffset? now = null, CancellationToken cancellationToken = default)
    {
        now ??= DateTimeOffset.Now;

        var queryDate = new DateTimeOffset(now.Value.Year, 1, 1, 0, 0, 0, now.Value.Offset);

        return _dbContext
          .Orders
          .Where(e => e.CreatedAt >= queryDate)
          .CountAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<ChartData<string>>> GetCustomerCountByGenderAsync(CancellationToken cancellationToken = default)
        => await _dbContext
              .Customers
              .GroupBy(e => e.Gender)
              .Select(g => new ChartData<string> { Label = g.Key, Value = g.Count() })
              .ToListAsync(cancellationToken);

    public async Task<IReadOnlyCollection<ChartData<string>>> GetUserRoleStatisticsByOrderAsync(CancellationToken cancellationToken = default)
        => await _dbContext
              .Orders
              .GroupBy(e => e.User.Role)
              .Select(g => new ChartData<string> { Label = g.Key, Value = g.Count() })
              .ToListAsync(cancellationToken);

    public async Task<IReadOnlyCollection<Product>> GetTopSellerProductsAsync(int count, CancellationToken cancellationToken = default)
        => await _dbContext
               .OrderProducts
               .Where(e => e.Order.Status == OrderStatus.PaymentReceived || e.Order.Status == OrderStatus.Completed)
               .GroupBy(e => e.ProductVariant.ProductId)
               .AsQueryable()
               .OrderByDescending(o => o.Sum(p => p.Quantity))
               .Take(count)
               .Select(e =>
                    e
                   .AsQueryable()
                   .Include(i => i.ProductVariant)
                   .ThenInclude(i => i.Product)
                   .ThenInclude(e => e.ProductDetailOptionValues.Where(e => e.ProductDetailOption.ProductOptionSubtype == ProductOptionSubtype.Main))
                   .First()
                   .ProductVariant
                   .Product
               )
               .ToListAsync(cancellationToken);

    public async Task<IReadOnlyCollection<ChartData<string>>> GetMostReviewedProductsAsync(int count, CancellationToken cancellationToken = default)
        => await _dbContext
               .Products
               .Where(e => e.ProductReviews.Count > 0)
               .OrderByDescending(o => o.ProductReviews.Count)
               .Take(count)
               .Select(e => new ChartData<string>
               {
                   Label = $"{e.ProductDetailOptionValues.First(v => v.ProductDetailOption.ProductOptionSubtype == ProductOptionSubtype.Main).Value} {e.Name}",
                   Value = e.ProductReviews.Count
               }).ToListAsync(cancellationToken);
}
