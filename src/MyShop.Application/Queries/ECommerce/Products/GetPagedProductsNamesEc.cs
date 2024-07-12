using MyShop.Application.Dtos;
using MyShop.Application.EndpointQueries.Interfaces;
using MyShop.Application.Responses;
using MyShop.Application.Validations.Interfaces;
using MyShop.Application.Validations.Validators;
using MyShop.Core.Validations;

namespace MyShop.Application.Queries.ECommerce.Products;
public sealed record GetPagedProductsNamesEc(
    int Take,
    string SearchPhrase
    ) : IQuery<ApiResponse<ValueDto<IReadOnlyCollection<string>>>>,
        ISearchQueryParams,
        IValidatable
{
    public void Validate(ICollection<ValidationMessage> validationMessages)
    {
        CustomValidators.Commons.MustBeIn(Take, [5, 10, 15], validationMessages, nameof(Take));

        if (string.IsNullOrWhiteSpace(SearchPhrase))
        {
            validationMessages.Add(new(
                nameof(SearchPhrase),
                [$"The field {nameof(SearchPhrase)} is required."]
                ));
        }
    }
}
