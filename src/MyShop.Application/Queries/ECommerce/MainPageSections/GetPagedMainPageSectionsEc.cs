using MyShop.Application.EndpointQueries.Interfaces;
using MyShop.Application.Responses;
using MyShop.Application.Validations.Interfaces;
using MyShop.Application.Validations.Validators;
using MyShop.Core.Dtos.ECommerce;
using MyShop.Core.Validations;

namespace MyShop.Application.Queries.ECommerce.MainPageSections;
public sealed record GetPagedMainPageSectionsEc(
    int PageNumber,
    int PageSize,
    int ProductCarouselItemsCount
    ) : IQuery<ApiPagedResponse<MainPageSectionEcDto>>,
        IPaginationQueryParams,
        IValidatable
{
    public void Validate(ICollection<ValidationMessage> validationMessages)
    {
        CustomValidators.PaginationParams.Validate(PageNumber, PageSize, validationMessages);
        CustomValidators.PaginationParams.PageSize.Validate(ProductCarouselItemsCount, validationMessages, nameof(ProductCarouselItemsCount));
    }
}
