namespace MyShop.Application.Dtos.ValidatorParameters;
public sealed record DateOnlyValidatorParameters
{
    public DateOnly? Min { get; init; }
    public DateOnly? Max { get; init; }
    public bool IsRequired { get; init; } = true;
}
