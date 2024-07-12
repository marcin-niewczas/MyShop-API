using MyShop.Application.ApplicationServices.Interfaces;
using MyShop.Application.Dtos.ManagementPanel.Dashboards;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.HelperModels;
using MyShop.Core.Models.Products;
using MyShop.Core.Utils;

namespace MyShop.Application.ApplicationServices;
public enum DashboardDataElement
{
    Unknown = 0,
    TotalSales,
    TotalProduct,
    TotalCategories,
    TotalCustomers,
    TotalOrders,
    YearSalesPerMonth,
    OrderStatistics,
    UserRoleStatisticsByOrder,
    CustomerGenderStatistics,
    TopSellerProducts,
    MostReviewedProducts
}

internal sealed class DashboardService(
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider
    ) : IDashboardService
{

    private readonly DashboardDataElement[] _dashboardDataElementsOrder = [
        DashboardDataElement.TotalSales,
        DashboardDataElement.TotalProduct,
        DashboardDataElement.TotalCustomers,
        DashboardDataElement.TotalOrders,
        DashboardDataElement.YearSalesPerMonth,
        DashboardDataElement.OrderStatistics,
        DashboardDataElement.UserRoleStatisticsByOrder,
        DashboardDataElement.CustomerGenderStatistics,
        DashboardDataElement.TopSellerProducts,
        DashboardDataElement.MostReviewedProducts,
    ];

    public async Task<ApiPagedResponse<BaseDashboardElementMpDto>> GetPagedDashboardDataAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default
        )
    {
        var result = new List<BaseDashboardElementMpDto>();

        var elements = _dashboardDataElementsOrder
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        foreach (var element in elements)
        {
            result.Add(await GetDashboardDataByElementAsync(element, cancellationToken));
        }

        return new ApiPagedResponse<BaseDashboardElementMpDto>(
            dtos: result,
            totalCount: _dashboardDataElementsOrder.Length,
            pageNumber: pageNumber,
            pageSize: pageSize
            );
    }


    private Task<BaseDashboardElementMpDto> GetDashboardDataByElementAsync(DashboardDataElement dashboardDataElement, CancellationToken cancellationToken = default)
        => dashboardDataElement switch
        {
            DashboardDataElement.TotalSales => GetTotalSalesAsync(cancellationToken),
            DashboardDataElement.TotalProduct => GetTotalProductVariantsCountAsync(cancellationToken),
            DashboardDataElement.TotalCategories => GetTotalCategoriesCountAsync(cancellationToken),
            DashboardDataElement.TotalCustomers => GetTotalCustomersAsync(cancellationToken),
            DashboardDataElement.TotalOrders => GetTotalOrdersAsync(cancellationToken),
            DashboardDataElement.YearSalesPerMonth => GetYearSalesPerMonthAsync(cancellationToken),
            DashboardDataElement.OrderStatistics => GetGroupOrderStatisticsAsync(cancellationToken),
            DashboardDataElement.UserRoleStatisticsByOrder => GetUserRoleStatisticsByOrderAsync(cancellationToken),
            DashboardDataElement.CustomerGenderStatistics => GetCustomerGenderStatisticsAsync(cancellationToken),
            DashboardDataElement.TopSellerProducts => GetTopSellerProductsAsync(cancellationToken: cancellationToken),
            DashboardDataElement.MostReviewedProducts => GetMostReviewedProductsAsync(cancellationToken: cancellationToken),
            _ => throw new NotImplementedException()
        };

    private async Task<BaseDashboardElementMpDto> GetTotalSalesAsync(CancellationToken cancellationToken = default)
    {
        var result = await unitOfWork
            .DashboardRepository
            .GetTotalSalesAsync(cancellationToken);

        return new OneValueDashboardElementMpDto(result, DashboardDataUnit.Currency, "Total Sales", "attach_money");
    }

    private async Task<BaseDashboardElementMpDto> GetTotalProductVariantsCountAsync(CancellationToken cancellationToken = default)
    {
        var result = await unitOfWork
            .DashboardRepository
            .GetTotalProductVariantsCountAsync(cancellationToken);

        return new OneValueDashboardElementMpDto(result, DashboardDataUnit.None, "Total Products", "inventory");
    }

    private async Task<BaseDashboardElementMpDto> GetTotalCategoriesCountAsync(CancellationToken cancellationToken = default)
    {
        var result = await unitOfWork
            .DashboardRepository
            .GetTotalCategoriesCountAsync(cancellationToken: cancellationToken);

        return new OneValueDashboardElementMpDto(result, DashboardDataUnit.None, "Categories", "category");
    }

    private async Task<BaseDashboardElementMpDto> GetTotalCustomersAsync(CancellationToken cancellationToken = default)
    {
        var result = await unitOfWork
            .DashboardRepository
            .GetTotalCustomersAsync(cancellationToken);

        return new OneValueDashboardElementMpDto(result, DashboardDataUnit.None, "Total Customers", "persons");
    }

    private async Task<BaseDashboardElementMpDto> GetTotalOrdersAsync(CancellationToken cancellationToken = default)
    {
        var result = await unitOfWork
            .DashboardRepository
            .GetTotalOrdersAsync(cancellationToken);

        return new OneValueDashboardElementMpDto(result, DashboardDataUnit.None, "Total Orders", "shopping_bag");
    }

    private async Task<BaseDashboardElementMpDto> GetYearSalesPerMonthAsync(CancellationToken cancellationToken = default)
    {
        var now = timeProvider.GetLocalNow();

        var result = await unitOfWork
            .DashboardRepository
            .GetYearSalesPerMonthAsync(now, cancellationToken);

        List<object> labels;
        List<object> values;


        var tempDate = new DateTime(now.Year, now.Month, 1, 0, 0, 0)
            .AddMonths(-11);


        ChartData<DateTimeOffset>? tempChartData;

        labels = [];
        values = [];


        for (int i = 0; i < 12; i++)
        {
            tempChartData = result.FirstOrDefault(r => r.Label.Month == tempDate.Month);

            if (tempChartData is null)
            {
                labels.Add(tempDate);
                values.Add(0);
            }
            else
            {
                labels.Add(tempDate);
                values.Add(tempChartData.Value);
            }

            tempDate = tempDate.AddMonths(1);
        }


        return new ChartDashboardElementMpDto(
            DashboardChartType.Line,
            labels,
            values,
            false,
            DashboardDataUnit.Date,
            DashboardDataUnit.Currency,
            "Monthly Sales",
            dashboardElementSize: DashboardElementSize.Large
            );
    }

    private async Task<BaseDashboardElementMpDto> GetGroupOrderStatisticsAsync(CancellationToken cancellationToken = default)
    {
        var todaySales = await unitOfWork.DashboardRepository.GetTodaySalesAsync(cancellationToken: cancellationToken);
        var todayOrderCount = await unitOfWork.DashboardRepository.GetTodayOrdersCountAsync(cancellationToken: cancellationToken);

        var thisWeekSales = await unitOfWork.DashboardRepository.GetThisWeekSalesAsync(cancellationToken: cancellationToken);
        var thisWeekOrderCount = await unitOfWork.DashboardRepository.GetThisWeekOrdersCountAsync(cancellationToken: cancellationToken);

        var thisMonthSales = await unitOfWork.DashboardRepository.GetThisMonthSalesAsync(cancellationToken: cancellationToken);
        var thisMonthOrderCount = await unitOfWork.DashboardRepository.GetThisMonthOrdersCountAsync(cancellationToken: cancellationToken);

        var thisYearSales = await unitOfWork.DashboardRepository.GetThisYearSalesAsync(cancellationToken: cancellationToken);
        var thisYearOrderCount = await unitOfWork.DashboardRepository.GetThisYearOrdersCountAsync(cancellationToken: cancellationToken);

        var data = new List<OneValueDashboardElementMpDto>()
        {
            new(todaySales,DashboardDataUnit.Currency, "Today Sales"),
            new(todayOrderCount, title: "Today Order Count"),

            new(thisWeekSales,DashboardDataUnit.Currency, " This Week Sales"),
            new(thisWeekOrderCount, title: "This Week Order Count"),

            new(thisMonthSales,DashboardDataUnit.Currency, " This Month Sales"),
            new(thisMonthOrderCount, title: "This Month Order Count"),

            new(thisYearSales,DashboardDataUnit.Currency, "This Year Sales"),
            new(thisYearOrderCount, title: "This Year Order Count"),
        };

        return new GroupValuesDashboardElementMpDto(
            data,
            GroupValuesDashboardElementType.Statistics,
            title: "Orders Statistics",
            icon: "attach_money",
            dashboardElementSize: DashboardElementSize.Large
            );
    }

    private async Task<BaseDashboardElementMpDto> GetUserRoleStatisticsByOrderAsync(CancellationToken cancellationToken = default)
    {
        var result = await unitOfWork.DashboardRepository.GetUserRoleStatisticsByOrderAsync(cancellationToken: cancellationToken);

        return new ChartDashboardElementMpDto(
            DashboardChartType.Pie,
            result.Select(r => r.Label),
            data: result.Select(r => r.Value),
            title: "Orders Count By User Role"
            );
    }

    private async Task<BaseDashboardElementMpDto> GetCustomerGenderStatisticsAsync(CancellationToken cancellationToken = default)
    {
        var result = await unitOfWork.DashboardRepository.GetCustomerCountByGenderAsync(cancellationToken: cancellationToken);

        return new ChartDashboardElementMpDto(
            DashboardChartType.Pie,
            result.Select(r => r.Label),
            data: result.Select(r => r.Value),
            title: "Customer Gender Statistics"
            );
    }

    private async Task<BaseDashboardElementMpDto> GetTopSellerProductsAsync(int count = 6, CancellationToken cancellationToken = default)
    {
        var products = await unitOfWork.DashboardRepository.GetTopSellerProductsAsync(count, cancellationToken: cancellationToken);
        var resourceType = nameof(Category.Products).ToEncodedName();

        return new GroupValuesDashboardElementMpDto(
            values: products.Select(p => new OneValueDashboardElementMpDto(
                $"{p.ProductDetailOptionValues.First().Value} {p.Name}",
                routerInfo: new(resourceType, p.Id.ToString())
                )),
            groupValuesDashboardElementType: GroupValuesDashboardElementType.Ranks,
            title: $"Best Sellers {nameof(Category.Products)}"
            );
    }

    private async Task<BaseDashboardElementMpDto> GetMostReviewedProductsAsync(int count = 6, CancellationToken cancellationToken = default)
    {
        var result = await unitOfWork.DashboardRepository.GetMostReviewedProductsAsync(count, cancellationToken: cancellationToken);

        return new ChartDashboardElementMpDto(
            DashboardChartType.Bar,
            result.Select(r => r.Label),
            data: result.Select(r => r.Value),
            title: "Most Reviewed Products",
            showDataLabels: true
            );
    }
}
