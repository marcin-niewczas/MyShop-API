using MyShop.Core.Abstractions;
using MyShop.Core.ValueObjects.Products;

namespace MyShop.Application.Dtos.ValidatorParameters.ManagementPanel;
public sealed class ProductValidatorParametersMpDto : IDto
{
    public StringValidatorParameters ModelNameParams { get; } = new()
    {
        MinLength = ProductName.MinLength,
        MaxLength = ProductName.MaxLength,
    };
    public StringValidatorParameters ProductDescriptionParams { get; } = new()
    {
        MinLength = ProductDescription.MinLength,
        MaxLength = ProductDescription.MaxLength,
        IsRequired = false,
    };
    public IReadOnlyCollection<string> AllowedDisplayProductTypes { get; }
        = DisplayProductType.AllowedValues.OfType<string>().ToArray();
}
