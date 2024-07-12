using Microsoft.EntityFrameworkCore;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Dtos.Account;
using MyShop.Core.Dtos.Auth;
using MyShop.Core.Dtos.ECommerce;
using MyShop.Core.Dtos.ManagementPanel;
using MyShop.Core.Dtos.Shared;
using MyShop.Core.HelperModels;
using MyShop.Core.Models.Orders;
using MyShop.Core.Models.Users;
using MyShop.Core.RepositoryQueryParams.Commons;
using MyShop.Core.RepositoryQueryParams.ECommerce;
using MyShop.Core.RepositoryQueryParams.ManagementPanel;
using MyShop.Core.ValueObjects.ProductOptions;
using MyShop.Infrastructure.DataAccessLayer.MainDatabase.Repositories.Utils;
using System.Linq.Expressions;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.Repositories;
internal sealed class OrderRepository(
    MainDbContext dbContext
    ) : BaseGenericRepository<Order>(dbContext), IOrderRepository
{
    public Task<OrderDetailsMpDto?> GetOrderDetailsMpAsync(
        Guid id,
        CancellationToken cancellationToken = default
        )
    {
        return _dbSet
            .Where(e => e.Id == id)
            .Select(e => new OrderDetailsMpDto
            {
                Id = e.Id,
                Email = e.Email,
                FirstName = e.FirstName,
                LastName = e.LastName,
                PhoneNumber = e.PhoneNumber,
                StreetName = e.StreetName,
                BuildingNumber = e.BuildingNumber,
                ApartmentNumber = e.ApartmentNumber,
                City = e.City,
                ZipCode = e.ZipCode,
                Country = e.Country,
                TotalPrice = e.OrderProducts.Sum(op => op.Quantity * op.Price),
                Status = e.Status,
                DeliveryMethod = e.DeliveryMethod,
                PaymentMethod = e.PaymentMethod,
                RedirectPaymentUri = e.RedirectPaymentUri,
                CreatedAt = e.CreatedAt,
                UpdatedAt = e.UpdatedAt,

                User = e.User.Role.HasCustomerPermission()
                    ? new CustomerMeDto
                    {
                        Id = e.User.Id,
                        FirstName = ((RegisteredUser)e.User).FirstName,
                        LastName = ((RegisteredUser)e.User).LastName,
                        PhoneNumber = ((RegisteredUser)e.User).PhoneNumber,
                        Email = ((RegisteredUser)e.User).Email,
                        UserRole = ((RegisteredUser)e.User).Role,
                        Gender = ((RegisteredUser)e.User).Gender,
                        DateOfBirth = ((RegisteredUser)e.User).DateOfBirth,
                        PhotoUrl = ((RegisteredUser)e.User).Photo != null
                            ? ((RegisteredUser)e.User).Photo!.Uri.ToString()
                            : null
                    }
                    : new UserDto
                    {
                        Id = e.UserId,
                        UserRole = e.User.Role
                    },

                OrderProducts = e.OrderProducts.Select(op => new OrderProductMpDto
                {
                    Id = op.Id,
                    ProductVariantId = op.ProductVariantId,
                    ProductId = op.ProductVariant.ProductId,
                    Name = string.Concat(
                            op.ProductVariant
                            .Product
                            .ProductDetailOptionValues
                            .First(v => v.ProductDetailOption.ProductOptionSubtype == ProductOptionSubtype.Main)
                            .Value,
                            " ",
                            op.ProductVariant.Product.Name
                            ),
                    CategoryHierarchyName = op.ProductVariant.Product.Category.HierarchyDetail.HierarchyName,
                    EncodedName = op.ProductVariant.EncodedName,
                    MainPhoto = op.ProductVariant
                            .PhotoItems
                            .Where(p => p.Position == 0)
                            .Select(p => new PhotoDto(p.ProductVariantPhoto.Uri, p.ProductVariantPhoto.Alt))
                            .FirstOrDefault(),
                    OrderId = op.OrderId,
                    Price = op.Price,
                    Quantity = op.Quantity,
                    PriceAll = op.Price * op.Quantity,
                    VariantOptionNameValues = op.ProductVariant
                            .Product
                            .ProductProductVariantOptions
                            .OrderBy(o => o.Position)
                            .Join(op.ProductVariant.ProductVariantOptionValues,
                                  k => k.ProductVariantOptionId,
                                  k => k.ProductOptionId,
                                  (_, v) => new OptionNameValue(v.ProductVariantOption.Name, v.Value))
                            .ToArray()

                }).ToArray(),

                OrderStatusHistories = e.OrderStatusHistories
                    .AsQueryable()
                    .OrderBy(o => o.CreatedAt)
                    .Select(h => new OrderStatusHistoryDto
                    {
                        Id = h.Id,
                        CreatedAt = h.CreatedAt,
                        UpdatedAt = h.UpdatedAt,
                        Status = h.Status,
                    }).ToArray(),
            })
            .AsSplitQuery()
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
    }


    public Task<PagedResult<PagedOrderMpDto>> GetPagedOrdersMpAsync(
        int pageNumber,
        int pageSize,
        GetPagedOrdersMpSortBy? sortBy,
        SortDirection? sortDirection,
        DateTimeOffset? fromDate,
        DateTimeOffset? toDate,
        bool inclusiveFromDate = true,
        bool inclusiveToDate = true,
        string? searchPhrase = null,
        CancellationToken cancellationToken = default
       )
    {
        var lowerSearchPhrase = searchPhrase?.ToLower();

        var baseQuery = _dbSet
                .Where(e => lowerSearchPhrase == null ||
                            e.Id.ToString().Contains(lowerSearchPhrase) ||
                            Convert.ToString(e.Email).ToLower().Contains(lowerSearchPhrase) ||
                            (Convert.ToString(e.FirstName).ToLower() + " " + Convert.ToString(e.LastName).ToLower()).Contains(lowerSearchPhrase) ||
                            Convert.ToString(e.PhoneNumber).ToLower().Contains(lowerSearchPhrase));


        if (fromDate is not null)
        {
            baseQuery = inclusiveFromDate switch
            {
                false => baseQuery.Where(e => fromDate == null || e.CreatedAt > fromDate),
                _ => baseQuery.Where(e => fromDate == null || e.CreatedAt >= fromDate),
            };
        }

        if (toDate is not null)
        {
            toDate = toDate.Value.AddDays(1);

            baseQuery = inclusiveToDate switch
            {
                false => baseQuery.Where(e => toDate == null || e.CreatedAt < toDate),
                _ => baseQuery.Where(e => toDate == null || e.CreatedAt <= toDate),
            };
        }

        var query = baseQuery.Select(e => new PagedOrderMpDto
        {
            Id = e.Id,
            Email = e.Email,
            FirstName = e.FirstName,
            LastName = e.LastName,
            PhoneNumber = e.PhoneNumber,
            StreetName = e.StreetName,
            BuildingNumber = e.BuildingNumber,
            ApartmentNumber = e.ApartmentNumber,
            City = e.City,
            ZipCode = e.ZipCode,
            Country = e.Country,
            TotalPrice = e.OrderProducts.Sum(op => op.Quantity * op.Price),
            Status = e.Status,
            DeliveryMethod = e.DeliveryMethod,
            PaymentMethod = e.PaymentMethod,
            CreatedAt = e.CreatedAt,
            UpdatedAt = e.UpdatedAt,
        });

        if (searchPhrase is not null && decimal.TryParse(searchPhrase, out var decimalValue))
        {
            query = query.Where(e => e.TotalPrice == decimalValue);
        }

        Expression<Func<PagedOrderMpDto, object?>> sortByExpression = sortBy switch
        {
            GetPagedOrdersMpSortBy.OrderId => o => o.Id,
            GetPagedOrdersMpSortBy.Email => o => o.Email,
            GetPagedOrdersMpSortBy.FirstName => o => o.FirstName,
            GetPagedOrdersMpSortBy.LastName => o => o.LastName,
            GetPagedOrdersMpSortBy.TotalPrice => o => o.TotalPrice,
            GetPagedOrdersMpSortBy.CreatedAt => o => o.CreatedAt,
            GetPagedOrdersMpSortBy.UpdatedAt => o => o.UpdatedAt,
            GetPagedOrdersMpSortBy.Status => o => o.Status,
            GetPagedOrdersMpSortBy.Street => o => o.StreetName,
            GetPagedOrdersMpSortBy.City => o => o.City,
            GetPagedOrdersMpSortBy.BuildingNumber => o => o.BuildingNumber,
            GetPagedOrdersMpSortBy.ApartmentNumber => o => o.ApartmentNumber,
            GetPagedOrdersMpSortBy.ZipCode => o => o.ZipCode,
            GetPagedOrdersMpSortBy.Country => o => o.Country,
            GetPagedOrdersMpSortBy.DeliveryMethod => o => o.DeliveryMethod,
            GetPagedOrdersMpSortBy.PaymentMethod => o => o.PaymentMethod,
            GetPagedOrdersMpSortBy.PhoneNumber => o => o.PhoneNumber,
            _ => o => o.CreatedAt,
        };

        query = sortDirection switch
        {
            SortDirection.Descendant => query.OrderByDescending(sortByExpression),
            _ => query.OrderBy(sortByExpression),
        };

        return query.ToPagedResultAsync(
            pageNumber: pageNumber,
            pageSize: pageSize,
            cancellationToken: cancellationToken
            );
    }

    public Task<OrderWithProductsEcDto?> GetFullOrderDataEcAsync(
    Guid orderId,
    Guid userId,
    CancellationToken cancellationToken = default
    )
    {
        var query = _dbSet
            .Where(e => e.Id == orderId && e.UserId == userId)
            .Select(e => new OrderWithProductsEcDto
            {
                Id = e.Id,
                Email = e.Email,
                FirstName = e.FirstName,
                LastName = e.LastName,
                PhoneNumber = e.PhoneNumber,
                StreetName = e.StreetName,
                BuildingNumber = e.BuildingNumber,
                ApartmentNumber = e.ApartmentNumber,
                City = e.City,
                ZipCode = e.ZipCode,
                Country = e.Country,
                TotalPrice = e.OrderProducts.Sum(op => op.Quantity * op.Price),
                Status = e.Status,
                DeliveryMethod = e.DeliveryMethod,
                PaymentMethod = e.PaymentMethod,
                RedirectPaymentUri = e.RedirectPaymentUri,
                CreatedAt = e.CreatedAt,
                UpdatedAt = e.UpdatedAt,
                OrderProducts = e.OrderProducts.Select(op => new OrderProductEcDto
                {
                    Name = string.Concat(
                        op.ProductVariant
                        .Product
                        .ProductDetailOptionValues
                        .First(v => v.ProductDetailOption.ProductOptionSubtype == ProductOptionSubtype.Main)
                        .Value,
                        " ",
                        op.ProductVariant.Product.Name
                        ),
                    CategoryHierarchyName = op.ProductVariant.Product.Category.HierarchyDetail.HierarchyName,
                    EncodedName = op.ProductVariant.EncodedName,
                    MainPhoto = op.ProductVariant
                        .PhotoItems
                        .Where(p => p.Position == 0)
                        .Select(p => new PhotoDto(p.ProductVariantPhoto.Uri, p.ProductVariantPhoto.Alt))
                        .FirstOrDefault(),
                    OrderId = op.OrderId,
                    Price = op.Price,
                    Quantity = op.Quantity,
                    PriceAll = op.Price * op.Quantity,
                    VariantOptionNameValues = op.ProductVariant
                        .Product
                        .ProductProductVariantOptions
                        .OrderBy(o => o.Position)
                        .Join(op.ProductVariant.ProductVariantOptionValues,
                              k => k.ProductVariantOptionId,
                              k => k.ProductOptionId,
                              (_, v) => new OptionNameValue(v.ProductVariantOption.Name, v.Value))
                        .ToArray()


                }).ToArray(),
                OrderStatusHistories = e.OrderStatusHistories
                    .AsQueryable()
                    .OrderBy(o => o.CreatedAt)
                    .Select(h => new OrderStatusHistoryDto
                    {
                        Id = h.Id,
                        CreatedAt = h.CreatedAt,
                        UpdatedAt = h.UpdatedAt,
                        Status = h.Status,
                    }).ToArray(),

            });



        return query
                .AsSplitQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<PagedResult<OrderAcDto>> GetPagedOrdersAcAsync(
        Guid userId,
        int pageNumber,
        int pageSize,
        GetPagedOrdersEcSortBy? sortBy,
        SortDirection? sortDirection,
        DateTimeOffset? fromDate,
        DateTimeOffset? toDate,
        bool inclusiveFromDate = true,
        bool inclusiveToDate = true,
        string? searchPhrase = null,
        CancellationToken cancellationToken = default
        )
    {
        if (toDate is not null)
        {
            toDate = toDate.Value.AddDays(1);
        }

        var baseQuery = _dbSet
            .Where(e => e.UserId == userId);

        baseQuery = inclusiveFromDate switch
        {
            true => baseQuery.Where(e => fromDate == null || e.CreatedAt >= fromDate),
            false => baseQuery.Where(e => fromDate == null || e.CreatedAt > fromDate)
        };

        baseQuery = inclusiveToDate switch
        {
            true => baseQuery.Where(e => toDate == null || e.CreatedAt <= toDate),
            false => baseQuery.Where(e => toDate == null || e.CreatedAt < toDate),
        };

        Expression<Func<Order, object?>> sortByExpression = sortBy switch
        {
            GetPagedOrdersEcSortBy.Status => x => x.Status,
            _ => x => x.CreatedAt
        };

        baseQuery = sortDirection switch
        {
            SortDirection.Ascendant => baseQuery.OrderBy(sortByExpression),
            _ => baseQuery.OrderByDescending(sortByExpression)
        };

        var dtosQuery = baseQuery.Select(e => new OrderAcDto()
        {
            Id = e.Id,
            CreatedAt = e.CreatedAt,
            UpdatedAt = e.UpdatedAt,
            Status = e.Status,
            TotalPrice = e.OrderProducts.Select(e => e.Price * e.Quantity).Sum(),
        }).AsQueryable()
          .Where(m => searchPhrase == null ||
               m.Id.ToString().ToLower().Contains(searchPhrase.ToLower()) ||
               m.TotalPrice.ToString().ToLower().Contains(searchPhrase.ToLower()));



        return dtosQuery.ToPagedResultAsync(
            pageNumber,
            pageSize,
            cancellationToken: cancellationToken
            );
    }
}
