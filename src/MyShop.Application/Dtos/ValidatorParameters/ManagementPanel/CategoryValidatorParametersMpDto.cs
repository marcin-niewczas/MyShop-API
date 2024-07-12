using MyShop.Core.Abstractions;
using MyShop.Core.ValueObjects.Categories;

namespace MyShop.Application.Dtos.ValidatorParameters.ManagementPanel;
public sealed record CategoryValidatorParametersMpDto : IDto
{
    public StringValidatorParameters CategoryNameParams { get; } = new()
    {
        MinLength = CategoryName.MinLength,
        MaxLength = CategoryName.MaxLength,
    };

    public int CategoryMaxLevel { get; } = CategoryLevel.Max;
}
