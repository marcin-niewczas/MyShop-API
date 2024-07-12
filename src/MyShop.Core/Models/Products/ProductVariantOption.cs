using MyShop.Core.Exceptions;
using MyShop.Core.HelperModels;
using MyShop.Core.Models.BaseEntities;
using MyShop.Core.Utils;
using MyShop.Core.ValueObjects.ProductOptions;

namespace MyShop.Core.Models.Products;
public sealed class ProductVariantOption : BaseProductOption
{
    public IReadOnlyCollection<ProductProductVariantOption> ProductProductVariantOptions { get; private set; } = default!;
    public IReadOnlyCollection<Product> Products { get; private set; } = default!;
    public override IReadOnlyCollection<ProductVariantOptionValue> ProductOptionValues => _productOptionValues;
    private List<ProductVariantOptionValue> _productOptionValues = default!;

    public ProductVariantOption(
        ProductOptionName name,
        ProductOptionSubtype productOptionSubtype,
        ProductOptionSortType productOptionSortType,
        IReadOnlyCollection<string> productVariantOptionValues
        ) : base(
            name,
            ProductOptionType.Variant,
            productOptionSubtype,
            productOptionSortType
            )
    {
        if (productVariantOptionValues is null)
            throw new ArgumentNullException(nameof(productVariantOptionValues), $"Parameter {nameof(productVariantOptionValues)} cannot be null.");

        if (productVariantOptionValues.HasDuplicate())
            throw new ArgumentException($"Parameter {nameof(productVariantOptionValues)} cannot contains duplicates.", nameof(productVariantOptionValues));

        _productOptionValues = productOptionSortType.Value switch
        {
            ProductOptionSortType.Custom => productVariantOptionValues.Select((v, i) => new ProductVariantOptionValue(v, Id, i)).ToList(),
            ProductOptionSortType.Alphabetically => productVariantOptionValues.Order().Select((v, i) => new ProductVariantOptionValue(v, Id, i)).ToList(),
            _ => throw new NotImplementedException(AllowedValuesError.Message<ProductOptionSortType>()),
        };
    }

    private ProductVariantOption() { }

    public void Update(ProductOptionName name, ProductOptionSortType productOptionSortType)
    {
        if (Name.ToLower().Equals(name.ToLower()) &&
            ProductOptionSortType.Value == productOptionSortType &&
            ProductOptionSubtype == ProductOptionSubtype.Main)
            throw new BadRequestException($"Nothing change in {nameof(ProductVariantOption).ToTitleCase()}.");

        if (Name.ToLower().Equals(name.ToLower()) &&
            ProductOptionSortType.Value == productOptionSortType &&
            ProductOptionSubtype == ProductOptionSubtype.Additional)
            throw new BadRequestException($"Nothing change in {nameof(ProductVariantOption).ToTitleCase()}.");

        Name = name;

        if (productOptionSortType.Value is ProductOptionSortType.Alphabetically &&
            ProductOptionSortType.Value is not ProductOptionSortType.Alphabetically)
        {
            if (ProductOptionValues is null)
                throw new InvalidOperationException($"{nameof(ProductOptionValues)} must be included for change {nameof(ProductOptionSortType)} on {ProductOptionSortType.Alphabetically}.");

            ProductOptionSortType = productOptionSortType;

            _productOptionValues = [.. _productOptionValues.OrderBy(v => v.Value.ToString())];

            var index = 0;

            foreach (var value in ProductOptionValues)
            {
                value.UpdatePosition(index++);
            }

            return;
        }

        ProductOptionSortType = productOptionSortType;
    }

    public void AddProductVariantOptionValue(ProductVariantOptionValue productVariantOptionValue)
    {
        if (_productOptionValues is null)
            throw new InvalidOperationException($"{nameof(ProductOptionValues)} must be included for add {nameof(ProductVariantOptionValue)}.");

        if (_productOptionValues.Any(v => v.Value.ToLower().Equals(productVariantOptionValue.Value.ToLower())))
            throw new BadRequestException($"{nameof(ProductVariantOptionValue.Value)} equal '{productVariantOptionValue.Value}' exist.");

        if (ProductOptionSortType.Value is ProductOptionSortType.Alphabetically)
        {
            _productOptionValues.Add(productVariantOptionValue);

            var index = 0;

            foreach (var value in _productOptionValues.OrderBy(v => v.Value.ToString()))
            {
                value.UpdatePosition(index++);
            }
        }

        if (ProductOptionSortType.Value is ProductOptionSortType.Custom)
        {
            productVariantOptionValue.UpdatePosition(_productOptionValues.Count switch
            {
                > 0 => _productOptionValues.Max(v => v.Position.Value) + 1,
                _ => 0
            });

            _productOptionValues.Add(productVariantOptionValue);
        }
    }

    public ProductVariantOptionValue UpdateProductVariantOptionValue(Guid id, ProductOptionValue value)
    {
        if (_productOptionValues is null)
            throw new InvalidOperationException($"{nameof(ProductOptionValues)} must be included for add {nameof(ProductVariantOptionValue)}.");

        if (_productOptionValues.Any(v => v.Value.Equals(value)))
            throw new BadRequestException($"{nameof(ProductVariantOptionValue.Value)} equal '{value}' exist.");

        var productVariantOptionValue = _productOptionValues.FirstOrDefault(v => v.Id == id)
            ?? throw new NotFoundException(nameof(ProductVariantOptionValue).ToTitleCase(), id);

        productVariantOptionValue.Update(value);

        if (ProductOptionSortType.Value == ProductOptionSortType.Alphabetically)
        {
            _productOptionValues = [.. _productOptionValues.OrderBy(v => v.Value.Value)];

            var index = 0;

            foreach (var optionValue in _productOptionValues)
            {
                optionValue.UpdatePosition(index++);
            }
        }

        return productVariantOptionValue;
    }

    public void UpdatePositionsOfProductVariantOptionValues(
        IReadOnlyCollection<ValuePosition<Guid>> updatePositionOfProductVariantOptionValues
        )
    {
        if (ProductOptionSortType != ProductOptionSortType.Custom)
            throw new BadRequestException(
                $"The {nameof(ProductVariantOption).ToTitleCase()} with {nameof(ProductOptionSortType).ToTitleCase()} equal {ProductOptionSortType.Custom} can change positions of {nameof(ProductOptionValues).ToTitleCase()}."
                );

        if (updatePositionOfProductVariantOptionValues.Count > _productOptionValues.Count)
            throw new BadRequestException(
                $"The {nameof(ProductVariantOption).ToTitleCase()} has {_productOptionValues.Count} {nameof(ProductOptionValues).ToTitleCase()}, but have been given {updatePositionOfProductVariantOptionValues.Count} {nameof(ProductOptionValues).ToTitleCase()} to update."
                );

        if (_productOptionValues is null)
            throw new InvalidOperationException($"{nameof(ProductOptionValues)} must be included for change positions of {nameof(ProductOptionValues)}.");

        if (_productOptionValues.Count is 0)
            throw new BadRequestException($"The {nameof(ProductVariantOption).ToTitleCase()} doesn't have {nameof(ProductOptionValues).ToTitleCase()}.");

        if (updatePositionOfProductVariantOptionValues.HasDuplicateBy(v => v.Position))
            throw new BadRequestException($"The {nameof(ProductVariantOptionValue.Position)} must be unique.");

        if (updatePositionOfProductVariantOptionValues.Any(v => v.Position < 0 || v.Position >= _productOptionValues.Count))
            throw new BadRequestException($"The {nameof(ProductVariantOptionValue.Position)} must be inclusive between 0 and {ProductOptionValues.Count - 1}");

        ProductVariantOptionValue toUpdateProductVariantOptionValue;
        int oldPosition;

        foreach (var value in updatePositionOfProductVariantOptionValues)
        {
            toUpdateProductVariantOptionValue = _productOptionValues.FirstOrDefault(v => v.Id == value.Value)
                ?? throw new BadRequestException($"The {nameof(ProductVariantOption).ToTitleCase()} haven't {nameof(ProductVariantOptionValue).ToTitleCase()} with {nameof(IEntity.Id)} equal {value.Value}.");

            oldPosition = toUpdateProductVariantOptionValue.Position;

            if (oldPosition == value.Position)
                throw new BadRequestException($"The {nameof(ProductVariantOptionValue.Position)} cannot be the same.");

            toUpdateProductVariantOptionValue.UpdatePosition(value.Position);
        }

        if (_productOptionValues.HasDuplicateBy(v => v.Position))
        {
            throw new BadRequestException($"Invalid positions. Positions must be unique.");
        }
    }
}
