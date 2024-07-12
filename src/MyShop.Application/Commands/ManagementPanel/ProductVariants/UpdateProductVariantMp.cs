using MyShop.Application.Validations.Interfaces;
using MyShop.Core.Validations;
using MyShop.Core.ValueObjects.Products;

namespace MyShop.Application.Commands.ManagementPanel.ProductVariants;
public sealed record UpdateProductVariantMp(
    Guid Id,
    int Quantity,
    decimal Price
    ) : ICommand,
        IValidatable
{
    public void Validate(ICollection<ValidationMessage> validationMessages)
    {
        ProductVariantQuantity.Validate(Quantity, validationMessages);
        ProductVariantPrice.Validate(Price, validationMessages);
    }
}
