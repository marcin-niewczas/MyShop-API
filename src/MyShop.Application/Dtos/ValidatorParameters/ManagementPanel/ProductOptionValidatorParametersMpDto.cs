using MyShop.Core.Abstractions;
using MyShop.Core.ValueObjects.ProductOptions;

namespace MyShop.Application.Dtos.ValidatorParameters.ManagementPanel;
public sealed record ProductOptionValidatorParametersMpDto : IDto
{
    public StringValidatorParameters ProductOptionNameParams { get; } = new()
    {
        MinLength = ProductOptionName.MinLength,
        MaxLength = ProductOptionName.MaxLength,
    };
    public StringValidatorParameters ProductOptionValueParams { get; } = new()
    {
        MinLength = ProductOptionValue.MinLength,
        MaxLength = ProductOptionValue.MaxLength,
    };
    public IReadOnlyCollection<string> AllowedProductOptionTypes { get; }
        = ProductOptionType.AllowedValues.OfType<string>().ToArray();
    public IReadOnlyCollection<string> AllowedProductOptionSubtypes { get; }
        = ProductOptionSubtype.AllowedValues.OfType<string>().ToArray();
    public IReadOnlyCollection<string> AllowedProductOptionSortTypes { get; }
        = ProductOptionSortType.AllowedValues.OfType<string>().ToArray();
}
