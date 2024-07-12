using MyShop.Application.Queries.ECommerce.MainPageSections;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Dtos.ECommerce;
using MyShop.Core.RepositoryQueryParams.ECommerce;
using MyShop.Core.ValueObjects.MainPageSections;

namespace MyShop.Application.QueryHandlers.ECommerce.MainPageSections;
internal sealed class GetPagedMainPageSectionsEcQueryHandler(
    IUnitOfWork unitOfWork
    ) : IQueryHandler<GetPagedMainPageSectionsEc, ApiPagedResponse<MainPageSectionEcDto>>
{
    public async Task<ApiPagedResponse<MainPageSectionEcDto>> HandleAsync(
        GetPagedMainPageSectionsEc query,
        CancellationToken cancellationToken = default
        )
    {
        var pagedResult = await unitOfWork.MainPageSectionRepository.GetPagedMainPageSectionsEcAsync(
              query.PageNumber,
              query.PageSize,
              cancellationToken
              );

        foreach (var result in pagedResult.Data.Where(i => i.MainPageSectionType == MainPageSectionType.WebsiteProductsCarouselSection).Cast<WebsiteProductsCarouselSectionEcDto>())
        {
            result.SetItems(
               (await unitOfWork.ProductVariantRepository.GetPagedDataByCategoryIdsAsync(
                    pageNumber: 1,
                    pageSize: query.ProductCarouselItemsCount,
                    sortBy: MapToSortBy(result.ProductsCarouselSectionType),
                    categoryIds: null,
                    productOptionParam: null,
                    minPrice: null,
                    maxPrice: null,
                    searchPhrase: null,
                    cancellationToken: cancellationToken
                    )).Data
                );
        }

        return new(
            dtos: pagedResult.Data,
            totalCount: pagedResult.TotalCount,
            pageNumber: query.PageNumber,
            pageSize: query.PageSize
            );
    }

    private static GetPagedProductsEcSortBy MapToSortBy(ProductsCarouselSectionType type)
        => type.Value switch
        {
            ProductsCarouselSectionType.Bestsellers => GetPagedProductsEcSortBy.Bestsellers,
            ProductsCarouselSectionType.Newest => GetPagedProductsEcSortBy.Newest,
            _ => throw new NotImplementedException()
        };
}
