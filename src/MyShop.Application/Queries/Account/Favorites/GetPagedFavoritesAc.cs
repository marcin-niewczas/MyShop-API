using MyShop.Application.EndpointQueries.Interfaces;
using MyShop.Application.Responses;
using MyShop.Application.Validations.Interfaces;
using MyShop.Application.Validations.Validators;
using MyShop.Core.Dtos.Shared;
using MyShop.Core.RepositoryQueryParams.Account;
using MyShop.Core.Validations;

namespace MyShop.Application.Queries.Account.Favorites;
public sealed record GetPagedFavoritesAc(
    int PageNumber,
    int PageSize,
    string? SortBy,
    string? SortDirection,
    string? SearchPhrase
    ) : IQuery<ApiPagedResponse<ProductItemDto>>,
        IPaginationQueryParams,
        ISortQueryParams,
        ISearchQueryParams,
        IValidatable
{
    public void Validate(ICollection<ValidationMessage> validationMessages)
    {
        CustomValidators.PaginationParams.Validate(PageNumber, PageSize, validationMessages);
        CustomValidators.SortParams.Validate<GetPagedFavoritesAcSortBy>(SortBy, SortDirection, validationMessages);
    }
}
