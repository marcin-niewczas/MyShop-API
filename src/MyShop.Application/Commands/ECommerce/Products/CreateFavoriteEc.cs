using Microsoft.AspNetCore.Mvc;
using MyShop.Application.Validations.Interfaces;
using MyShop.Core.Validations;

namespace MyShop.Application.Commands.ECommerce.Products;
public sealed record CreateFavoriteEc(
    [FromRoute] string EncodedName
    ) : ICommand,
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
