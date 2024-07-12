using MyShop.Application.EndpointQueries.Interfaces;
using MyShop.Application.Responses;
using MyShop.Application.Validations.Interfaces;
using MyShop.Application.Validations.Validators;
using MyShop.Core.Dtos.ManagementPanel;
using MyShop.Core.RepositoryQueryParams.ManagementPanel;
using MyShop.Core.Validations;

namespace MyShop.Application.Queries.ManagementPanel.Categories;
public sealed record GetPagedProductCategoriesByCategoryRootIdMp(
    Guid RootId,
    int PageNumber,
    int PageSize,
    string? SortBy,
    string? SortDirection,
    string? SearchPhrase
    ) : IQuery<ApiPagedResponse<CategoryMpDto>>,
        IPaginationQueryParams,
        ISortQueryParams,
        ISearchQueryParams,
        IValidatable
{
    public void Validate(ICollection<ValidationMessage> validationMessages)
    {
        CustomValidators.PaginationParams.Validate(PageNumber, PageSize, validationMessages);
        CustomValidators.SortParams.Validate<GetPagedCategoriesMpSortBy>(SortBy, SortDirection, validationMessages);
    }
}
