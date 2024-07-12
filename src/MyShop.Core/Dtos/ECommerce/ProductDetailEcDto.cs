using MyShop.Core.Abstractions;
using MyShop.Core.Dtos.Shared;
using MyShop.Core.HelperModels;
using MyShop.Core.Models.Products;
using MyShop.Core.ValueObjects.ProductOptions;
using System.Text.Json.Serialization;

namespace MyShop.Core.Dtos.ECommerce;

[JsonDerivedType(typeof(MainVariantProductDetailEcDto))]
[JsonDerivedType(typeof(AllVariantProductDetailEcDto))]
public abstract record BaseProductDetailEcDto : IDto
{
    public string ModelName { get; }
    public abstract string FullName { get; }
    public string? Description { get; }
    public string EncodedProductName { get; }
    public string CategoryHierarchyName { get; }
    public string CategoryEncodedName { get; }
    public Guid ProductId { get; }
    public string DisplayProductPer { get; }
    public int ProductReviewsCount { get; }
    public int SumProductReviewsRate { get; }
    public OptionNameValue MainDetailOptions { get; }
    public IReadOnlyCollection<OptionNameValue> AdditionalDetailOptions { get; }
    public double AvarageProductReviewsRate
        => ProductReviewsCount == 0 ? 0 : Math.Round((double)SumProductReviewsRate / ProductReviewsCount, 1);
    public IReadOnlyCollection<PhotoDto> Photos { get; protected set; } = default!;
    public OptionNameValue MainVariantOption { get; protected set; } = default!;

    public BaseProductDetailEcDto(
        string encodedProductName,
        Product product,
        int productReviewsCount,
        int sumProductReviewsRate
        )
    {
        EncodedProductName = encodedProductName;
        ModelName = product.Name;
        Description = product.Description;
        CategoryHierarchyName = product.Category.HierarchyDetail.HierarchyName;
        CategoryEncodedName = product.Category.HierarchyDetail.EncodedHierarchyName;
        ProductId = product.Id;
        DisplayProductPer = product.DisplayProductType;
        ProductReviewsCount = productReviewsCount;
        SumProductReviewsRate = sumProductReviewsRate;

        var mainDetailOptionValue = product
            .ProductProductDetailOptionValues
            .First(v => v.ProductDetailOptionValue.ProductDetailOption.ProductOptionSubtype == ProductOptionSubtype.Main)
            .ProductDetailOptionValue;

        MainDetailOptions = new(
            mainDetailOptionValue.ProductDetailOption.Name,
            mainDetailOptionValue.Value
            );

        AdditionalDetailOptions = product
            .ProductProductDetailOptionValues
            .Where(v => v.ProductDetailOptionValue.ProductDetailOption.ProductOptionSubtype == ProductOptionSubtype.Additional)
            .Select(v => new OptionNameValue(
                v.ProductDetailOptionValue.ProductDetailOption.Name,
                v.ProductDetailOptionValue.Value
                ))
            .ToArray();
    }
}

public sealed record MainVariantProductDetailEcDto : BaseProductDetailEcDto
{
    public override string FullName
        => $"{MainDetailOptions.Value} {ModelName}";
    public decimal MinPrice { get; }
    public decimal MaxPrice { get; }
    public bool IsStablePrice
        => MinPrice == MaxPrice;
    public string CurrentProductEncodedName { get; }
    public IReadOnlyCollection<CurrentVariantByMainEcDto> CurrentVariants { get; }
    public IReadOnlyCollection<VariantByEc<OptionNameValue>> OtherVariants { get; }
    public IReadOnlyCollection<AvailableOptionByMainDto> AvailableOptions { get; }

    public MainVariantProductDetailEcDto(
        string encodedProductName,
        Product product,
        int productReviewsCount,
        int sumProductReviewsRate,
        IReadOnlyCollection<AvailableOptionByMainDto> availableOptions,
        decimal minPrice,
        decimal maxPrice,
        IReadOnlyCollection<CurrentVariantByMainEcDto> currentVariants,
        IReadOnlyCollection<VariantByEc<OptionNameValue>> otherVariants,
        string currentProductEncodedName
        ) : base(
            encodedProductName,
            product,
            productReviewsCount,
            sumProductReviewsRate
            )
    {
        MinPrice = minPrice;
        MaxPrice = maxPrice;
        CurrentVariants = currentVariants;
        OtherVariants = otherVariants;
        MainVariantOption = CurrentVariants.First().MainVariantOption;
        Photos = CurrentVariants.First().Photos;
        CurrentProductEncodedName = currentProductEncodedName;
        AvailableOptions = availableOptions;
    }
}

public sealed record AllVariantProductDetailEcDto : BaseProductDetailEcDto
{
    public override string FullName
        => (AdditionalVariantOptions.Count > 0) switch
        {
            true => $"{MainDetailOptions.Value} {ModelName} {string.Join('/', AdditionalVariantOptions.Select(v => v.Value))} {MainVariantOption.Value}",
            _ => $"{MainDetailOptions.Value} {ModelName} {MainVariantOption.Value}"
        };
    public IReadOnlyCollection<OptionNameValue> AdditionalVariantOptions { get; }
    public IReadOnlyCollection<OptionNameValue> AllCurrentVariantOptions { get; }
    public decimal Price { get; }
    public bool? LastItemsInStock { get; }
    public Guid ProductVariantId { get; }
    public IReadOnlyCollection<VariantByEc<IReadOnlyCollection<OptionNameValue>>> AllVariants { get; }
    public IReadOnlyCollection<AvailableOptionByAllEc> AvailableOptions { get; set; }


    public AllVariantProductDetailEcDto(
        string encodedProductName,
        Product product,
        int productReviewsCount,
        int sumProductReviewsRate,
        IReadOnlyCollection<AvailableOptionByAllEc> availableOptions,
        CurrentVariantByAllEc currentProductVariant,
        IReadOnlyCollection<VariantByEc<IReadOnlyCollection<OptionNameValue>>> allVariants
        ) : base(
            encodedProductName,
            product,
            productReviewsCount,
            sumProductReviewsRate
            )
    {
        ProductVariantId = currentProductVariant.Id;
        Price = currentProductVariant.Price;
        LastItemsInStock = currentProductVariant.LastItemsInStock;
        Photos = currentProductVariant.Photos;


        var mainVariantOptionValue = currentProductVariant
            .ProductOptionNameValues
            .First(v => v.ProductOptionSubtype == ProductOptionSubtype.Main);

        MainVariantOption = mainVariantOptionValue;

        AdditionalVariantOptions = currentProductVariant
            .ProductOptionNameValues
            .Where(v => v.ProductOptionSubtype == ProductOptionSubtype.Additional)
            .ToArray();

        AllCurrentVariantOptions = [MainVariantOption, .. AdditionalVariantOptions];
        AllVariants = allVariants;
        AvailableOptions = availableOptions;
    }
}

public sealed record CurrentVariantByMainEcDto(
    Guid Id,
    decimal Price,
    bool? LastItemsInStock,
    OptionNameValue MainVariantOption,
    IReadOnlyCollection<OptionNameValue> VariantValues,
    IReadOnlyCollection<PhotoDto> Photos
    );

public sealed record CurrentVariantByAllEc(
    Guid Id,
    decimal Price,
    bool? LastItemsInStock,
    IReadOnlyCollection<ProductOptionNameValue> ProductOptionNameValues,
    IReadOnlyCollection<PhotoDto> Photos
    );

public sealed record AvailableOptionByMainDto(
    string Name,
    IReadOnlyCollection<string> VariantValues
    );

public sealed record AvailableOptionByAllEc(
    string Name,
    IReadOnlyCollection<VariantByEc<string>> VariantValues
    );


public sealed record VariantByEc<TValue>
{
    public required string EncodedName { get; init; }
    public required TValue Value { get; init; }
}

public sealed record VariantByAllEcDto
{
    public required string EncodedName { get; init; }
    public required IReadOnlyCollection<OptionNameValue> OptionNameValues { get; init; }

}

public sealed record VariantByMainEcDto
{
    public required string EncodedName { get; init; }
    public required OptionNameValue OptionNameValue { get; init; }

}

public sealed record ProductOptionNameValue(
    string Name,
    string Value,
    ProductOptionSubtype ProductOptionSubtype
    ) : OptionNameValue(Name, Value);
