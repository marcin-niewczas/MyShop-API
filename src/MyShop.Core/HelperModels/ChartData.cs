namespace MyShop.Core.HelperModels;
public sealed record ChartData<TLabel>
{
    public required TLabel Label { get; init; }
    public required object Value { get; init; }
}
