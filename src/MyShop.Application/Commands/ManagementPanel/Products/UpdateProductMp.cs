using MyShop.Application.Validations.Interfaces;
using MyShop.Core.Validations;
using MyShop.Core.ValueObjects.Products;
using ValueObjects = MyShop.Core.ValueObjects;

namespace MyShop.Application.Commands.ManagementPanel.Products;
public sealed record UpdateProductMp(
    Guid Id,
    string ProductName,
    string DisplayProductType,
    string Description
    ) : ICommand, 
        IValidatable
{
    public void Validate(ICollection<ValidationMessage> validationMessages)
    {
        ValueObjects.Products.ProductName.Validate(ProductName, validationMessages);
        Validations.Validators.CustomValidators.Enums.MustBeIn<DisplayProductType>(
            DisplayProductType,
            validationMessages,
            nameof(DisplayProductType)
            );
        ProductDescription.Validate(Description, validationMessages);
    }
}
