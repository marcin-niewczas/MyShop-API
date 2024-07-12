namespace MyShop.Core.HelperModels;
public sealed record Changed<T, TSource>(
    T From,
    T To,
    TSource Source
    );
