using MyShop.Core.Abstractions;
using MyShop.Core.Validations;
using MyShop.Core.ValueObjects.Products;

namespace MyShop.Application.Dtos.ValidatorParameters.ManagementPanel;
public sealed class ProductVariantValidatorParametersMpDto : IDto
{
    public StringValidatorParameters ProductVariantQuantityParams { get; } = new()
    {
        RegexPattern = CustomRegex.NaturalNumberPattern,
    };
    public StringValidatorParameters PriceParams { get; } = new()
    {
        RegexPattern = CustomRegex.PricePattern,
    };
    public PhotoValidatorParameters PhotoParams { get; }
        = new PhotoValidatorParameters();
    public int MaxPhotos { get; }
        = ProductVariantPhotoItemPosition.Max + 1;
}
