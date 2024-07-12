namespace MyShop.Core.HelperModels;
public sealed record ValuePosition<TValue>(
    TValue Value,
    int Position
    );
