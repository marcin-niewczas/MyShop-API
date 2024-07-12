using MyShop.Core.HelperModels;
using MyShop.Core.Models.Products;

namespace MyShop.Core.Abstractions.Repositories;
public interface IDashboardRepository
{
    Task<IReadOnlyCollection<ChartData<string>>> GetCustomerCountByGenderAsync(CancellationToken cancellationToken = default);
    Task<int> GetThisMonthOrdersCountAsync(DateTimeOffset? now = null, CancellationToken cancellationToken = default);
    Task<decimal> GetThisMonthSalesAsync(DateTimeOffset? now = null, CancellationToken cancellationToken = default);
    Task<int> GetThisWeekOrdersCountAsync(DateTimeOffset? now = null, CancellationToken cancellationToken = default);
    Task<decimal> GetThisWeekSalesAsync(DateTimeOffset? now = null, CancellationToken cancellationToken = default);
    Task<int> GetThisYearOrdersCountAsync(DateTimeOffset? now = null, CancellationToken cancellationToken = default);
    Task<decimal> GetThisYearSalesAsync(DateTimeOffset? now = null, CancellationToken cancellationToken = default);
    Task<int> GetTodayOrdersCountAsync(DateTimeOffset? now = null, CancellationToken cancellationToken = default);
    Task<decimal> GetTodaySalesAsync(DateTimeOffset? now = null, CancellationToken cancellationToken = default);
    Task<int> GetTotalCategoriesCountAsync(int maxCategoryLevel = 2, CancellationToken cancellationToken = default);
    Task<int> GetTotalCustomersAsync(CancellationToken cancellationToken = default);
    Task<int> GetTotalOrdersAsync(CancellationToken cancellationToken = default);
    Task<int> GetTotalProductVariantsCountAsync(CancellationToken cancellationToken = default);
    Task<decimal> GetTotalSalesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<ChartData<string>>> GetUserRoleStatisticsByOrderAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<ChartData<DateTimeOffset>>> GetYearSalesPerMonthAsync(DateTimeOffset? now = null, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Product>> GetTopSellerProductsAsync(int count, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<ChartData<string>>> GetMostReviewedProductsAsync(int count, CancellationToken cancellationToken = default);
}
