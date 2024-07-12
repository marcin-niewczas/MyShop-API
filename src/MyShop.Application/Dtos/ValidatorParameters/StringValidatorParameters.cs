namespace MyShop.Application.Dtos.ValidatorParameters;
public sealed record StringValidatorParameters
{
    public int? MinLength { get; init; }
    public int? MaxLength { get; init; }
    public bool IsRequired { get; init; } = true;
    public string? RegexPattern { get; init; }
    public string? ErrorMessage { get; init; }
}
