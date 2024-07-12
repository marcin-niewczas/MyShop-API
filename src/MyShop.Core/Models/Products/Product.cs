using MyShop.Core.Exceptions;
using MyShop.Core.HelperModels;
using MyShop.Core.Models.BaseEntities;
using MyShop.Core.Utils;
using MyShop.Core.ValueObjects.ProductOptions;
using MyShop.Core.ValueObjects.Products;

namespace MyShop.Core.Models.Products;
public sealed class Product : BaseTimestampEntity
{
    public ProductName Name { get; private set; } = default!;
    public DisplayProductType DisplayProductType { get; private set; } = default!;
    public ProductDescription Description { get; private set; } = default!;
    public string EncodedName { get; private set; } = default!;
    public Category Category { get; private set; } = default!;
    public Guid CategoryId { get; private set; }
    public IReadOnlyCollection<ProductProductVariantOption> ProductProductVariantOptions => _productProductVariantOptions;
    private readonly List<ProductProductVariantOption> _productProductVariantOptions = default!;
    public IReadOnlyCollection<ProductVariantOption> ProductVariantOptions { get; private set; } = default!;
    public IReadOnlyCollection<ProductVariant> ProductVariants { get; private set; } = default!;
    public IReadOnlyCollection<ProductProductDetailOptionValue> ProductProductDetailOptionValues => _productProductDetailOptionValues;
    private readonly List<ProductProductDetailOptionValue> _productProductDetailOptionValues = default!;
    public IReadOnlyCollection<ProductDetailOptionValue> ProductDetailOptionValues { get; private set; } = default!;
    public IReadOnlyCollection<ProductReview> ProductReviews { get; private set; } = default!;

    public Product(
        ProductName name,
        DisplayProductType displayProductType,
        ProductDescription description,
        Guid categoryId,
        IReadOnlyCollection<ValuePosition<ProductDetailOptionValue>> valuePositionsOfProductDetailOptionValues,
        IReadOnlyCollection<ValuePosition<ProductVariantOption>> valuePositionsOfProductVariantOptions
        )
    {
        Name = name;
        DisplayProductType = displayProductType;
        Description = description;

        if (categoryId == Guid.Empty)
            throw new ArgumentException(categoryId.ToString(), nameof(categoryId));

        CategoryId = categoryId;

        _productProductDetailOptionValues = [];
        AddRangeProductProductDetailOptionValues(valuePositionsOfProductDetailOptionValues);

        _productProductVariantOptions = [];
        AddRangeProductVariantOptions(valuePositionsOfProductVariantOptions);

        EncodeName();
    }

    private Product() { }

    public void AddRangeProductVariantOptions(
        IReadOnlyCollection<ValuePosition<ProductVariantOption>> valuePositionsOfProductVariantOptions
        )
    {
        ArgumentNullException.ThrowIfNull(DisplayProductType);

        var mainProductVariantOption = valuePositionsOfProductVariantOptions.SingleWhenOnly(o => o.Value.ProductOptionSubtype == ProductOptionSubtype.Main)
            ?? throw new BadRequestException($"The {nameof(Product)} must have exactly one {nameof(ProductVariantOption).ToTitleCase()} with {ProductOptionSubtype.Main} type.");

        if (mainProductVariantOption.Position is not 0)
            throw new BadRequestException(
                $"The {nameof(ProductProductDetailOptionValue.Position)} of {ProductOptionSubtype.Main} {nameof(ProductVariantOption)} must be equal 0."
                );

        if (
            DisplayProductType == DisplayProductType.MainVariantOption
            && !valuePositionsOfProductVariantOptions.Any(o => o.Value.ProductOptionSubtype == ProductOptionSubtype.Additional)
            )
        {
            throw new BadRequestException(
                $"The {nameof(Product)} must have exactly one {nameof(ProductVariantOption).ToTitleCase()} with {ProductOptionSubtype.Main} type " +
                $"and at least one {ProductOptionSubtype.Additional} {nameof(ProductVariantOption).ToTitleCase()}, if {nameof(DisplayProductType)} is set as {DisplayProductType.MainVariantOption}."
                );
        }

        if (valuePositionsOfProductVariantOptions.HasDuplicateBy(v => v.Value.Id))
            throw new BadRequestException($"The {nameof(Product)} cannot has duplicates in {nameof(ProductVariantOptions).ToTitleCase()}.");

        if (valuePositionsOfProductVariantOptions.HasDuplicateBy(e => e.Position))
            throw new BadRequestException($"The {nameof(ProductProductVariantOption.Position)} must be unique for each chosen {nameof(ProductVariantOption)}.");

        var correctPositionsArray = Enumerable.Range(0, valuePositionsOfProductVariantOptions.Count).ToArray();

        if (!correctPositionsArray.All(c => valuePositionsOfProductVariantOptions.Any(v => v.Position == c)))
            throw new BadRequestException(
                $"The {nameof(ProductProductDetailOptionValue.Position)}s of {nameof(ProductDetailOptionValue)}s is invalid, must contains in [ ${string.Join(", ", correctPositionsArray)} ]."
                );

        var productProductVariantOptions = valuePositionsOfProductVariantOptions
            .Select(p => new ProductProductVariantOption(Id, p.Value.Id, p.Position));

        _productProductVariantOptions.AddRange(productProductVariantOptions);
    }

    public void AddRangeProductProductDetailOptionValues(
        IReadOnlyCollection<ValuePosition<ProductDetailOptionValue>> valuePositionsOfProductDetailOptionValues
        )
    {
        ArgumentNullException.ThrowIfNull(valuePositionsOfProductDetailOptionValues);

        if (valuePositionsOfProductDetailOptionValues.Any(v => v.Value.ProductDetailOption is null))
            throw new ArgumentException(
                $"The {nameof(ProductDetailOption)} must be included for method {nameof(AddRangeProductProductDetailOptionValues)}.", nameof(valuePositionsOfProductDetailOptionValues)
                );

        if (valuePositionsOfProductDetailOptionValues.HasDuplicateBy(e => e.Value.ProductOptionId))
            throw new BadRequestException(
                $"The {nameof(Product)} must have one {nameof(ProductDetailOptionValue).ToTitleCase()} for specific {nameof(ProductDetailOption)}."
                );

        var mainProductDetailOptionValue = valuePositionsOfProductDetailOptionValues.SingleWhenOnly(o => o.Value.ProductDetailOption.ProductOptionSubtype == ProductOptionSubtype.Main)
            ?? throw new BadRequestException(
                $"The {nameof(Product)} must have exactly one {nameof(ProductVariantOption).ToTitleCase()} with {ProductOptionSubtype.Main} type."
                );

        if (mainProductDetailOptionValue.Position is not 0)
            throw new BadRequestException(
                $"The {nameof(ProductProductDetailOptionValue.Position)} of {ProductOptionSubtype.Main} {nameof(ProductDetailOptionValue)} must be equal 0."
                );

        if (valuePositionsOfProductDetailOptionValues.HasDuplicateBy(e => e.Position))
            throw new BadRequestException($"The {nameof(ProductProductDetailOptionValue.Position)} must be unique for each chosen {nameof(ProductDetailOptionValue)}.");

        var correctPositionsArray = Enumerable.Range(0, valuePositionsOfProductDetailOptionValues.Count).ToArray();

        if (!correctPositionsArray.All(c => valuePositionsOfProductDetailOptionValues.Any(v => v.Position == c)))
            throw new BadRequestException(
                $"The {nameof(ProductProductDetailOptionValue.Position)}s of {nameof(ProductDetailOptionValue)}s is invalid, must contains in [ ${string.Join(", ", correctPositionsArray)} ]."
                );

        var productProductDetailOptionValues = valuePositionsOfProductDetailOptionValues
            .Select(p => new ProductProductDetailOptionValue(Id, p.Value.Id, p.Position));

        _productProductDetailOptionValues.AddRange(productProductDetailOptionValues);
    }

    public void ChangePositionsOfProductProductDetailOptions(
        IReadOnlyCollection<ValuePosition<Guid>> idPositions
        )
    {
        var minPosition = ProductOptionPosition.Min + 1;
        var maxPositions = _productProductDetailOptionValues.Count - 1;

        if (idPositions.Any(x => x.Position < minPosition || x.Position > maxPositions))
        {
            throw new BadRequestException(
                $"The {nameof(ProductProductVariantOption.Position)} for {nameof(Product)} '{Id}' must be inclusive between {minPosition} and {maxPositions}."
                );
        }

        foreach (var idPosition in idPositions)
        {
            (_productProductDetailOptionValues
                .FirstOrDefault(x => x.Id == idPosition.Value)
                ?? throw new NotFoundException($"Not found {nameof(ProductDetailOptionValue)} '{idPosition.Value}' in {nameof(Product)} '{Id}'."))
                .UpdatePosition(idPosition.Position);
        }

        var allowedPositions = Enumerable.Range(ProductOptionPosition.Min, _productProductDetailOptionValues.Count);

        if (_productProductDetailOptionValues.HasDuplicateBy(v => v.Position) || _productProductDetailOptionValues.Any(v => !allowedPositions.Contains(v.Position)))
        {
            throw new BadRequestException($"Invalid positions. Positions must be unique and not contains gaps.");
        }
    }

    public void ChangePositionsOfProductProductVariantOptions(
        IReadOnlyCollection<ValuePosition<Guid>> idPositions
        )
    {
        var minPosition = ProductOptionPosition.Min + 1;
        var maxPositions = _productProductVariantOptions.Count - 1;

        if (idPositions.Any(x => x.Position < minPosition || x.Position > maxPositions))
        {
            throw new BadRequestException(
                $"The {nameof(ProductProductVariantOption.Position)} for {nameof(Product)} '{Id}' must be inclusive between {minPosition} and {maxPositions}."
                );
        }

        foreach (var idPosition in idPositions)
        {
            (_productProductVariantOptions
                .FirstOrDefault(x => x.Id == idPosition.Value)
                ?? throw new NotFoundException($"Not found {nameof(ProductVariantOption)} '{idPosition.Value}' in {nameof(Product)} '{Id}'."))
                .UpdatePosition(idPosition.Position);
        }

        var allowedPositions = Enumerable.Range(ProductOptionPosition.Min, _productProductVariantOptions.Count);

        if (_productProductVariantOptions.HasDuplicateBy(v => v.Position) || _productProductVariantOptions.Any(v => !allowedPositions.Contains(v.Position)))
        {
            throw new BadRequestException($"Invalid positions. Positions must be unique and not contains gaps.");
        }

        foreach (var variant in ProductVariants)
        {
            variant.RebuildSortPriorityAndEncodedName();
        }
    }

    public void RemoveProductProductDetailOptionValue(
        Guid id
        )
    {
        var entityToRemove = _productProductDetailOptionValues.FirstOrDefault(x => x.Id == id)
            ?? throw new NotFoundException(nameof(ProductProductDetailOptionValue), id);

        if (entityToRemove.ProductDetailOptionValue.ProductDetailOption.ProductOptionSubtype == ProductOptionSubtype.Main)
        {
            throw new BadRequestException(
                $"The {nameof(ProductProductDetailOptionValue)} cannot be removed, beacuse is related with {nameof(ProductDetailOption)} with {ProductOptionSubtype.Main} subtype."
                );
        }

        foreach (var item in _productProductDetailOptionValues.Where(v => v.Position > entityToRemove.Position && v.Id != entityToRemove.Id))
        {
            item.UpdatePosition(item.Position - 1);
        }

        _productProductDetailOptionValues.Remove(entityToRemove);
    }

    public void Update(
        ProductName productName,
        DisplayProductType displayProductType,
        ProductDescription description
        )
    {
        if (Name == productName &&
            DisplayProductType == displayProductType &&
            Description == description)
        {
            throw new BadRequestException($"Nothing change in {nameof(Product)}.");
        }

        if (Name == productName &&
           DisplayProductType == displayProductType &&
           Description != description)
        {
            Description = description;
            return;
        }

        Name = productName;
        DisplayProductType = displayProductType;
        Description = description;

        foreach (var variant in ProductVariants)
        {
            variant.RebuildEncodedName();
        }
    }

    private void EncodeName()
        => EncodedName = Name.ToEncodedName();
}
