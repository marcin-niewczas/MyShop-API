using MyShop.Application.Dtos.ManagementPanel.ProductReviews;
using MyShop.Application.EndpointQueries.Interfaces;
using MyShop.Application.Responses;
using MyShop.Application.Validations.Interfaces;
using MyShop.Application.Validations.Validators;
using MyShop.Core.RepositoryQueryParams.Shared;
using MyShop.Core.Validations;
using ValueObjects = MyShop.Core.ValueObjects;

namespace MyShop.Application.Queries.ManagementPanel.Products;
public sealed record GetPagedProductReviewsByProductIdMp(
    Guid Id,
    int PageNumber,
    int PageSize,
    string? SortBy,
    string? SortDirection,
    int? ProductReviewRate
    ) : IQuery<ApiPagedResponse<ProductReviewMpDto>>,
        IPaginationQueryParams,
        ISortQueryParams,
        IValidatable
{
    public void Validate(ICollection<ValidationMessage> validationMessages)
    {
        CustomValidators.PaginationParams.Validate(PageNumber, PageSize, validationMessages);
        CustomValidators.SortParams.Validate<GetPagedProductReviewsSortBy>(SortBy, SortDirection, validationMessages);

        if (ProductReviewRate is not null)
        {
            ValueObjects.ProductReviews.ProductReviewRate.Validate(ProductReviewRate, validationMessages);
        }
    }
}
