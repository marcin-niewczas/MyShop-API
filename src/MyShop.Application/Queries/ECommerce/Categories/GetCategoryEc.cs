using Microsoft.AspNetCore.Mvc;
using MyShop.Application.Dtos.ECommerce.Categories;
using MyShop.Application.Responses;
using MyShop.Application.Validations.Interfaces;
using MyShop.Core.Validations;

namespace MyShop.Application.Queries.ECommerce.Categories;
public sealed record GetCategoryEc(
    [FromRoute] string EncodedName
    ) : IQuery<ApiResponse<CategoryEcDto>>,
        IValidatable
{
    public void Validate(ICollection<ValidationMessage> validationMessages)
    {
        if (string.IsNullOrWhiteSpace(EncodedName))
        {
            validationMessages.Add(new(
                nameof(EncodedName),
                [$"The field {nameof(EncodedName)} is required."]
                ));
        }
    }
}
