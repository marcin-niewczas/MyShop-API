using MyShop.Core.Exceptions;
using MyShop.Core.Exceptions.ModelsException;
using MyShop.Core.HelperModels;
using MyShop.Core.Models.BaseEntities;
using MyShop.Core.Models.Orders;
using MyShop.Core.Models.Photos;
using MyShop.Core.Models.ShoppingCarts;
using MyShop.Core.Utils;
using MyShop.Core.ValueObjects.ProductOptions;
using MyShop.Core.ValueObjects.Products;

namespace MyShop.Core.Models.Products;
public sealed class ProductVariant : BaseTimestampEntity
{
    public Guid SkuId { get; } = Guid.NewGuid();
    public string EncodedName { get; private set; } = default!;
    public ProductVariantQuantity Quantity { get; private set; } = default!;
    public ProductVariantPrice Price { get; private set; } = default!;
    public Product Product { get; private set; } = default!;
    public Guid ProductId { get; private set; }
    public string SortPriority { get; private set; } = default!;
    public IReadOnlyCollection<ProductVariantOptionValue> ProductVariantOptionValues => _productVariantOptionValues;
    private readonly List<ProductVariantOptionValue> _productVariantOptionValues = default!;
    public IReadOnlyCollection<ProductVariantPhoto> Photos { get; private set; } = default!;
    public IReadOnlyCollection<ProductVariantPhotoItem> PhotoItems => _photoItems;
    private readonly List<ProductVariantPhotoItem> _photoItems = default!;
    public IReadOnlyCollection<ShoppingCartItem> ShoppingCartItems => _shoppingCartItems;
    private readonly List<ShoppingCartItem> _shoppingCartItems = default!;
    public IReadOnlyCollection<OrderProduct> OrderProducts => _orderProducts;
    private readonly List<OrderProduct> _orderProducts = default!;

    private ProductVariant() { }

    public ProductVariant(
        ProductVariantQuantity quantity,
        ProductVariantPrice price,
        Product product,
        Guid productId,
        IReadOnlyCollection<ProductVariantOptionValue> productVariantOptionValues
        )
    {
        ArgumentNullException.ThrowIfNull(nameof(product), $"The {nameof(Product)} cannot be null.");

        if (productId == Guid.Empty)
            throw new ArgumentException($"The {nameof(ProductId)} cannot be empty.", nameof(productId));

        if (productVariantOptionValues.IsNullOrEmpty())
            throw new ArgumentNullException(nameof(productVariantOptionValues), $"The {nameof(ProductVariantOptionValues)} cannot be null.");

        if (product.ProductProductVariantOptions is null)
            throw new ArgumentException($"The {nameof(Product)} must included {nameof(Product.ProductProductVariantOptions)}.", nameof(product));

        if (product.ProductVariantOptions is null)
            throw new ArgumentException($"The {nameof(Product)} must included {nameof(Product.ProductVariantOptions)}.", nameof(product));

        if (product.ProductVariantOptions is { Count: <= 0 } ||
           product.ProductVariantOptions.HasEntityDuplicates() ||
           !product.ProductVariantOptions.HasExactlyOne(o => o.ProductOptionSubtype == ProductOptionSubtype.Main) ||
           (product.DisplayProductType == DisplayProductType.MainVariantOption && !product.ProductVariantOptions.HasAtLeastOne(o => o.ProductOptionSubtype == ProductOptionSubtype.Additional)))
        {
            throw new InvalidDataInDatabaseException($"The {nameof(Product)} with '{product.Id}' has invalid {nameof(Product.ProductVariantOptions)}.");
        }

        if (productVariantOptionValues.HasDuplicateBy(v => v.ProductOptionId) &&
            !productVariantOptionValues.All(v => product.ProductProductVariantOptions.Any(o => o.ProductVariantOptionId == v.ProductOptionId)))
        {
            throw new BadRequestException($"Invalid {nameof(ProductVariantOptionValue).ToTitleCase()} Id/Ids.");
        }

        Quantity = quantity ?? throw new ArgumentNullException(nameof(quantity));
        Price = price ?? throw new ArgumentNullException(nameof(price));
        Product = product;
        ProductId = productId;
        _productVariantOptionValues = [.. productVariantOptionValues];
        RebuildSortPriority();
        EncodeName();
    }

    public ProductVariant(
        ProductVariantQuantity quantity,
        ProductVariantPrice price,
        Product product,
        Guid productId,
        IReadOnlyCollection<ProductVariantOptionValue> productVariantOptionValues,
        IReadOnlyCollection<ValuePosition<Guid>> photosIdPositions
        ) : this(
            quantity,
            price,
            product,
            productId,
            productVariantOptionValues
            )
    {
        ArgumentNullException.ThrowIfNull(photosIdPositions);

        _photoItems = [];

        foreach (var photo in photosIdPositions)
        {
            AddProductVariantPhotoItem(photo);
        }
    }

    public void RebuildSortPriority()
    {
        SortPriority = string.Join(
            ":",
            Product
            .ProductProductVariantOptions
            .Select(o => string.Concat(
                o.Position,
                "-",
                _productVariantOptionValues.Single(v => v.ProductOptionId == o.ProductVariantOptionId).Position
                )
            ));
    }

    public void RebuildSortPriorityAndEncodedName()
    {
        var orderedValues = Product.ProductProductVariantOptions.Join(
            _productVariantOptionValues,
            k => k.ProductVariantOptionId,
            k => k.ProductOptionId,
            (ppvo, value) => new { Position = ppvo.Position.Value, Value = value }
            ).OrderBy(o => o.Position)
            .ToList();

        SortPriority = string.Join(
            ":",
            orderedValues
            .Select(o => string.Concat(
                o.Position,
                "-",
                o.Value.Position
                )
            ));

        var baseName = string.Concat(
           Product.ProductDetailOptionValues.First(v => v.ProductDetailOption.ProductOptionSubtype == ProductOptionSubtype.Main).Value,
           " ",
           Product.Name
           );

        EncodedName = $"{baseName} {Product.DisplayProductType.Value switch
        {
            DisplayProductType.MainVariantOption => orderedValues[0].Value.Value,
            DisplayProductType.AllVariantOptions => string.Join(
                    "-",
                    orderedValues.Select(o => o.Value.Value)
                ),
            _ => throw new NotImplementedException()
        }}".ToEncodedName();
    }

    public void RebuildEncodedName()
    {
        var baseName = string.Concat(
            Product.ProductDetailOptionValues.First(v => v.ProductDetailOption.ProductOptionSubtype == ProductOptionSubtype.Main).Value,
            " ",
            Product.Name
            );

        EncodedName = $"{baseName}-{Product.DisplayProductType.Value switch
        {
            DisplayProductType.MainVariantOption => _productVariantOptionValues
                    .First(v => v.ProductVariantOption.ProductOptionSubtype == ProductOptionSubtype.Main)
                    .Value,
            DisplayProductType.AllVariantOptions => string.Join(
                    "-",
                    Product
                    .ProductProductVariantOptions
                    .Select(o => _productVariantOptionValues.Single(v => v.ProductOptionId == o.ProductVariantOptionId).Value)
                ),
            _ => throw new NotImplementedException()
        }}".ToEncodedName();
    }

    public void UpdateSortPriority(
        IReadOnlyCollection<ValuePosition<Guid>> positionOfProductOptionValues
        )
    {
        if (positionOfProductOptionValues is null or { Count: <= 0 })
        {
            throw new ArgumentNullException(nameof(positionOfProductOptionValues));
        }

        var splittedSortPriority = SortPriority.Split(":").ToList();

        foreach (var positionToUpdate in positionOfProductOptionValues)
        {
            var productVariantOptionValue = ProductVariantOptionValues.Single(v => v.Id == positionToUpdate.Value);
            var currentPositionProductVariantOption = Product
                .ProductProductVariantOptions
                .First(o => o.ProductVariantOptionId == productVariantOptionValue.ProductOptionId)
                .Position;

            var currentPositionInSortPriority = splittedSortPriority[currentPositionProductVariantOption];

            var splittedCurrentPositionInSortPriority = currentPositionInSortPriority.Split("-").ToList();

            splittedCurrentPositionInSortPriority[1] = positionToUpdate.Position.ToString();

            splittedSortPriority[currentPositionProductVariantOption] = string.Join("-", splittedCurrentPositionInSortPriority);

            SortPriority = string.Join(":", splittedSortPriority);
        }
    }

    public void UpdateQuantity(ProductVariantQuantity quantity)
    {
        Quantity = quantity ?? throw new ArgumentNullException(nameof(quantity));
    }

    public void UpdatePrice(ProductVariantPrice price)
    {
        Price = price ?? throw new ArgumentNullException(nameof(price));
    }

    public void RestoreQuantity(ProductVariantQuantity quantity)
    {
        ArgumentNullException.ThrowIfNull(quantity, nameof(quantity));

        Quantity += quantity;
    }

    public ProductVariantPhotoItem AddProductVariantPhotoItem(
        ValuePosition<Guid> photoIdPosition
        )
    {
        ArgumentNullException.ThrowIfNull(photoIdPosition);

        if (_photoItems is null)
            throw new InvalidOperationException($"The {nameof(Photos)} must be included.");

        if (_photoItems.Any(p => p.ProductVariantPhotoId == photoIdPosition.Value))
            throw new BadRequestException($"The {nameof(ProductVariant)} contains {nameof(ProductVariantPhoto)} '{photoIdPosition.Value}'.");

        if (_photoItems.Any(p => p.Position == photoIdPosition.Position))
            throw new ArgumentException($"Photos in {nameof(ProductVariant)} must have unique {nameof(ProductVariantPhotoItemPosition)}s.");

        var maxCount = ProductVariantPhotoItemPosition.Max + 1;

        if (_photoItems.Count >= maxCount)
        {
            throw new ArgumentException($"Each {nameof(ProductVariant)} can have max. {maxCount} photos.");
        }

        var productVariantPhotoItem = new ProductVariantPhotoItem(this, photoIdPosition.Value, photoIdPosition.Position);

        _photoItems.Add(productVariantPhotoItem);

        if (_photoItems.HasDuplicateBy(p => p.Position))
        {
            throw new BadRequestException($"Invalid positions. Positions must be unique.");
        }

        return productVariantPhotoItem;
    }

    public ProductVariantPhotoItem AddProductVariantPhotoItem(
        ProductVariantPhotoItemPosition position,
        ProductVariantPhoto photo
        )
    {
        ArgumentNullException.ThrowIfNull(position);
        ArgumentNullException.ThrowIfNull(photo);

        if (_photoItems is null)
            throw new InvalidOperationException($"The {nameof(Photos)} must be included.");

        if (_photoItems.Any(p => p.Position == position))
            throw new ArgumentException($"Photos in {nameof(ProductVariant)} must have unique {nameof(ProductVariantPhotoItemPosition)}s.");

        var maxCount = ProductVariantPhotoItemPosition.Max + 1;

        if (_photoItems.Count >= maxCount)
        {
            throw new ArgumentException($"Each {nameof(ProductVariant)} can have max. {maxCount} photos.");
        }

        var productVariantPhotoItem = new ProductVariantPhotoItem(this, photo, position);

        _photoItems.Add(productVariantPhotoItem);

        if (_photoItems.HasDuplicateBy(p => p.Position))
        {
            throw new BadRequestException($"Invalid positions. Positions must be unique.");
        }

        return productVariantPhotoItem;
    }

    public ProductVariantPhotoItem RemoveProductVariantPhotoItem(
        Guid id
        )
    {
        if (_photoItems is null)
            throw new InvalidOperationException($"The {nameof(Photos)} must be included.");

        var photoItemToDelete = _photoItems.FirstOrDefault(p => p.Id.Equals(id))
             ?? throw new ArgumentNullException($"Not found {nameof(ProductVariantPhotoItem)} '{id}' in {nameof(ProductVariant)} '{Id}'.");

        foreach (var photoItem in _photoItems.Where(p => p.Position > photoItemToDelete.Position))
        {
            photoItem.UpdatePosition(photoItem.Position - 1);
        }

        _photoItems.Remove(photoItemToDelete);

        return photoItemToDelete;
    }

    public void UpdatePositionsOfProductVariantPhotoItem(
        IReadOnlyCollection<ValuePosition<Guid>> idPositions
        )
    {
        if (_photoItems is null)
            throw new InvalidOperationException($"The {nameof(Photos)} must be included.");

        foreach (var idPosition in idPositions)
        {
            (_photoItems.FirstOrDefault(p => p.Id.Equals(idPosition.Value))
                ?? throw new NotFoundException($"Not found {nameof(ProductVariantPhotoItem)} '{idPosition.Value}' in {nameof(ProductVariant)} '{Id}'."))
                .UpdatePosition(idPosition.Position);
        }

        if (_photoItems.HasDuplicateBy(p => p.Position))
        {
            throw new BadRequestException($"Invalid positions. Positions must be unique.");
        }
    }

    private void EncodeName()
       => EncodedName = Product.DisplayProductType.Value switch
       {
           DisplayProductType.MainVariantOption => GetEncodeMainVariantOptionName(),
           DisplayProductType.AllVariantOptions => GetEncodeAllVariantOptionsName(),
           _ => throw new InvalidDataInDatabaseException(AllowedValuesError.Message<DisplayProductType>()),
       };


    private string GetEncodeAllVariantOptionsName()
    {
        if (_productVariantOptionValues.IsNullOrEmpty())
        {
            throw new ProductVariantException(
                            $"The {nameof(Product)} with {nameof(IEntity.Id)} '{Product.Id}' must contain exactly one {nameof(ProductVariantOptionValue)} with '{ProductOptionSubtype.Main}' type."
                            );
        }

        var productOptionVariantValuesAsString = string.Join(' ', _productVariantOptionValues.Select(v => v.Value.ToString()));

        var mainDetailOptionValue = Product.ProductDetailOptionValues.FirstOrDefault(v => v.ProductDetailOption.ProductOptionSubtype == ProductOptionSubtype.Main)
            ?? throw new ProductVariantException($"The {nameof(Product)} must contain at least one {nameof(ProductDetailOptionValue)} with '{ProductOptionSubtype.Main}' type.");

        return $"{mainDetailOptionValue.Value} {Product.Name} {productOptionVariantValuesAsString}"
            .ToTrimmedString()
            .Replace(" ", "-")
            .ToLower();
    }

    private string GetEncodeMainVariantOptionName()
    {
        var mainProductVariantOption = Product.ProductVariantOptions
            .SingleWhenOnly(o => o.ProductOptionSubtype == ProductOptionSubtype.Main)
            ?? throw new InvalidDataInDatabaseException(
                $"The {nameof(Product)} with {nameof(IEntity.Id)} '{Product.Id}' must contain exactly one {nameof(ProductVariantOption)} with '{ProductOptionSubtype.Main}' type."
                );

        var mainProductVariantOptionValue = _productVariantOptionValues
            .SingleWhenOnly(v => v.ProductOptionId == mainProductVariantOption.Id)
            ?? throw new InvalidDataInDatabaseException(
                $"The {nameof(Product)} with {nameof(IEntity.Id)} '{Product.Id}' must contain exactly one {nameof(ProductVariantOption)} with '{ProductOptionSubtype.Main}' type."
                );

        var mainDetailOptionValue = Product.ProductDetailOptionValues.FirstOrDefault(v => v.ProductDetailOption.ProductOptionSubtype == ProductOptionSubtype.Main)
            ?? throw new ProductVariantException($"The {nameof(Product)} must contain at least one {nameof(ProductDetailOptionValue)} with '{ProductOptionSubtype.Main}' type.");

        return $"{mainDetailOptionValue.Value} {Product.Name} {mainProductVariantOptionValue.Value}"
            .ToTrimmedString()
            .Replace(" ", "-")
            .ToLower();
    }

    public void ReplaceMainDetailOptionValueInEncodedName(
        ProductOptionValue oldValue,
        ProductOptionValue newValue
        )
    {
        var encodedOldValue = oldValue.Value.ToEncodedName();
        var encodedNewValue = newValue.Value.ToEncodedName();

        EncodedName = EncodedName.ReplaceFirst(encodedOldValue, encodedNewValue);
    }

    public string GetProductVariantFullName()
    {
        var mainDetailOptionValue = Product
            .ProductDetailOptionValues
            .First(v => v.ProductDetailOption.ProductOptionSubtype == ProductOptionSubtype.Main).Value;

        var variantOptionsValuesLabel = string.Join(
            "/",
            Product
            .ProductProductVariantOptions
            .OrderBy(o => o.Position.Value)
            .Join(
                ProductVariantOptionValues,
                x => x.ProductVariantOptionId,
                x => x.ProductOptionId,
                (_, v) => v.Value
                )
            );

        return $"{mainDetailOptionValue} {Product.Name} {variantOptionsValuesLabel}";
    }
}
