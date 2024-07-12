using MyShop.Core.Exceptions;
using MyShop.Core.HelperModels;
using MyShop.Core.Models.BaseEntities;
using MyShop.Core.Utils;
using MyShop.Core.ValueObjects.ProductOptions;

namespace MyShop.Core.Models.Products;
public sealed class ProductDetailOption : BaseProductOption
{
    public override IReadOnlyCollection<ProductDetailOptionValue> ProductOptionValues => _productOptionValues;
    private readonly List<ProductDetailOptionValue> _productOptionValues = default!;

    public ProductDetailOption(
        ProductOptionName name,
        ProductOptionSubtype productOptionSubtype,
        ProductOptionSortType productOptionSortType,
        IReadOnlyCollection<string> productDetailOptionValues
        ) : base(
            name,
            ProductOptionType.Detail,
            productOptionSubtype,
            productOptionSortType
            )
    {
        if (productDetailOptionValues is null)
            throw new ArgumentNullException(nameof(productDetailOptionValues), $"Parameter {nameof(productDetailOptionValues)} cannot be null.");

        if (productDetailOptionValues.HasDuplicate())
            throw new ArgumentException($"Parameter {nameof(productDetailOptionValues)} cannot contains duplicates.", nameof(productDetailOptionValues));

        _productOptionValues = productOptionSortType.Value switch
        {
            ProductOptionSortType.Custom => productDetailOptionValues.Select((v, i) => new ProductDetailOptionValue(v, Id, i)).ToList(),
            ProductOptionSortType.Alphabetically => productDetailOptionValues.Order().Select((v, i) => new ProductDetailOptionValue(v, Id, i)).ToList(),
            _ => throw new NotImplementedException(AllowedValuesError.Message<ProductOptionSortType>()),
        };
    }

    private ProductDetailOption() { }

    public void Update(ProductOptionName name, ProductOptionSortType productOptionSortType)
    {
        if (Name.ToLower().Equals(name.ToLower()) &&
            ProductOptionSortType.Value == productOptionSortType &&
            ProductOptionSubtype == ProductOptionSubtype.Main
            )
            throw new BadRequestException($"Nothing change in {nameof(ProductDetailOption).ToTitleCase()}.");

        if (Name.ToLower().Equals(name.ToLower()) &&
            ProductOptionSortType.Value == productOptionSortType &&
            ProductOptionSubtype == ProductOptionSubtype.Additional)
            throw new BadRequestException($"Nothing change in {nameof(ProductDetailOption).ToTitleCase()}.");

        Name = name;

        if (productOptionSortType.Value is ProductOptionSortType.Alphabetically &&
            ProductOptionSortType.Value is not ProductOptionSortType.Alphabetically)
        {
            if (_productOptionValues is null)
                throw new InvalidOperationException(
                    $"{nameof(ProductOptionValues)} must be included for change {nameof(ProductOptionSortType)} on {ProductOptionSortType.Alphabetically}."
                    );

            ProductOptionSortType = productOptionSortType;

            var index = 0;

            foreach (var value in _productOptionValues.OrderBy(v => v.Value))
            {
                value.UpdatePosition(index++);
            }

            return;
        }

        ProductOptionSortType = productOptionSortType;
    }

    public void AddProductDetailOptionValue(ProductDetailOptionValue productDetailOptionValue)
    {
        if (_productOptionValues is null)
            throw new InvalidOperationException($"{nameof(ProductOptionValues)} must be included for add {nameof(ProductVariantOptionValue)}.");

        if (_productOptionValues.Any(v => v.Value.ToLower().Equals(productDetailOptionValue.Value.ToLower())))
            throw new BadRequestException($"{nameof(ProductDetailOptionValue.Value)} equal '{productDetailOptionValue.Value}' exist.");

        if (ProductOptionSortType.Value is ProductOptionSortType.Alphabetically)
        {
            _productOptionValues.Add(productDetailOptionValue);

            var index = 0;

            foreach (var value in _productOptionValues.OrderBy(v => v.Value.ToString()))
            {
                value.UpdatePosition(index++);
            }
        }

        if (ProductOptionSortType.Value is ProductOptionSortType.Custom)
        {
            productDetailOptionValue.UpdatePosition(_productOptionValues.Count switch
            {
                > 0 => _productOptionValues.Max(v => v.Position.Value) + 1,
                _ => 0
            });

            _productOptionValues.Add(productDetailOptionValue);
        }
    }

    public ProductDetailOptionValue UpdateProductDetailOptionValue(Guid id, ProductOptionValue value)
    {
        if (_productOptionValues is null)
            throw new InvalidOperationException($"{nameof(ProductOptionValues)} must be included for add {nameof(ProductVariantOptionValue)}.");

        if (_productOptionValues.Any(v => v.Value.Equals(value)))
            throw new BadRequestException($"{nameof(ProductDetailOptionValue.Value)} equal '{value}' exist.");

        var productDetailOptionValue = _productOptionValues.FirstOrDefault(v => v.Id == id)
            ?? throw new NotFoundException(nameof(ProductDetailOptionValue).ToTitleCase(), id);

        productDetailOptionValue.Update(value);

        if (ProductOptionSortType.Value == ProductOptionSortType.Alphabetically)
        {
            var index = 0;

            foreach (var optionValue in _productOptionValues.OrderBy(v => v.Value.Value))
            {
                optionValue.UpdatePosition(index++);
            }
        }

        return productDetailOptionValue;
    }

    public void UpdatePositionsOfProductDetailOptionValues(
        IReadOnlyCollection<ValuePosition<Guid>> updatePositionOfProductDetailOptionValues
        )
    {
        if (ProductOptionSortType != ProductOptionSortType.Custom)
            throw new BadRequestException(
                $"The {nameof(ProductDetailOption).ToTitleCase()} with {nameof(ProductOptionSortType).ToTitleCase()} equal {ProductOptionSortType.Custom} can change positions of {nameof(ProductOptionValues).ToTitleCase()}."
                );

        if (updatePositionOfProductDetailOptionValues.Count > _productOptionValues.Count)
            throw new BadRequestException(
                $"The {nameof(ProductDetailOption).ToTitleCase()} has {_productOptionValues.Count} {nameof(ProductOptionValues).ToTitleCase()}, but have been given {updatePositionOfProductDetailOptionValues.Count} {nameof(ProductOptionValues).ToTitleCase()} to update."
                );

        if (_productOptionValues is null)
            throw new InvalidOperationException($"{nameof(ProductOptionValues)} must be included for change positions of {nameof(ProductOptionValues)}.");

        if (_productOptionValues.Count is 0)
            throw new BadRequestException($"The {nameof(ProductVariantOption).ToTitleCase()} doesn't have {nameof(ProductOptionValues).ToTitleCase()}.");

        if (updatePositionOfProductDetailOptionValues.HasDuplicateBy(v => v.Position))
            throw new BadRequestException($"The {nameof(ProductDetailOptionValue.Position)} must be unique.");

        if (updatePositionOfProductDetailOptionValues.Any(v => v.Position < 0 || v.Position >= _productOptionValues.Count))
            throw new BadRequestException($"The {nameof(ProductVariantOptionValue.Position)} must be inclusive between 0 and {_productOptionValues.Count - 1}");

        ProductDetailOptionValue toUpdateProductDetailOptionValue;
        int oldPosition;

        foreach (var value in updatePositionOfProductDetailOptionValues)
        {
            toUpdateProductDetailOptionValue = _productOptionValues.FirstOrDefault(v => v.Id == value.Value)
                ?? throw new BadRequestException($"The {nameof(ProductDetailOption).ToTitleCase()} haven't {nameof(ProductDetailOptionValue).ToTitleCase()} with {nameof(IEntity.Id)} equal {value.Value}.");

            oldPosition = toUpdateProductDetailOptionValue.Position;

            if (oldPosition == value.Position)
                throw new BadRequestException($"The {nameof(ProductDetailOptionValue.Position)} cannot be the same.");

            toUpdateProductDetailOptionValue.UpdatePosition(value.Position);
        }

        if (_productOptionValues.HasDuplicateBy(v => v.Position))
        {
            throw new BadRequestException($"Invalid positions. Positions must be unique.");
        }
    }
}
