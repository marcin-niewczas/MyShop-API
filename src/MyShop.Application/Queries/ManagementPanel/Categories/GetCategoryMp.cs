using MyShop.Application.EndpointQueries.Interfaces;
using MyShop.Application.Responses;
using MyShop.Application.Validations.Interfaces;
using MyShop.Application.Validations.Validators;
using MyShop.Core.Dtos.ManagementPanel;
using MyShop.Core.RepositoryQueryParams.ManagementPanel;
using MyShop.Core.Validations;

namespace MyShop.Application.Queries.ManagementPanel.Categories;
public sealed record GetCategoryMp(
    Guid Id,
    string QueryType
    ) : IQuery<ApiResponse<CategoryMpDto>>,
        IQueryTypeParams,
        IValidatable
{
    public void Validate(ICollection<ValidationMessage> validationMessages)
    {
        CustomValidators.Enums.IsInEnum<GetCategoryMpQueryType>(QueryType, validationMessages, nameof(QueryType));
    }
}
