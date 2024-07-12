using MyShop.Application.Dtos.ECommerce.Categories;
using MyShop.Application.EndpointQueries.Interfaces;
using MyShop.Application.Responses;
using MyShop.Application.Validations.Interfaces;
using MyShop.Application.Validations.Validators;
using MyShop.Core.RepositoryQueryParams.Shared;
using MyShop.Core.Validations;

namespace MyShop.Application.Queries.ECommerce.Categories;
public sealed record GetPagedCategoriesEc(
    int PageNumber,
    int PageSize,
    string? SearchPhrase,
    string QueryType
    ) : IQuery<ApiPagedResponse<CategoryEcDto>>,
        IPaginationQueryParams,
        ISearchQueryParams,
        IValidatable,
        IQueryTypeParams
{
    public void Validate(ICollection<ValidationMessage> validationMessages)
    {
        CustomValidators.PaginationParams.Validate(PageNumber, PageSize, validationMessages);
        CustomValidators.Enums.IsInEnum<GetPagedCategoriesQueryType>(QueryType, validationMessages, nameof(QueryType));
    }
}
