using MyShop.Application.Responses;
using MyShop.Application.Validations.Interfaces;
using MyShop.Application.Validations.Validators;
using MyShop.Core.HelperModels;
using MyShop.Core.Utils;
using MyShop.Core.Validations;
using MyShop.Core.ValueObjects.Products;

namespace MyShop.Application.Commands.ManagementPanel.ProductVariants;
public sealed record CreateProductVariantMp(
    Guid ProductId,
    int Quantity,
    decimal Price,
    IReadOnlyCollection<Guid> ProductVariantOptionValueIds,
    IReadOnlyCollection<ValuePosition<Guid>>? PhotosIdPositions
    ) : ICommand<ApiIdResponse>, IValidatable
{
    public void Validate(ICollection<ValidationMessage> validationMessages)
    {
        ProductVariantQuantity.Validate(Quantity, validationMessages);
        ProductVariantPrice.Validate(Price, validationMessages);

        if (PhotosIdPositions is not null)
        {
            CustomValidators.HelperModels.Validate(
                idPositions: PhotosIdPositions,
                validationMessages: validationMessages,
                maxPosition: ProductVariantPhotoItemPosition.Max,
                isRequired: false,
                paramName: nameof(PhotosIdPositions)
                );

            var maxCount = ProductVariantPhotoItemPosition.Max + 1;

            if (PhotosIdPositions.Count > maxCount)
            {
                validationMessages.Add(new(
                    nameof(PhotosIdPositions),
                    [$"The collection {nameof(PhotosIdPositions)} must contains max. {maxCount} items."]
                    ));
            }
        }

        if (ProductVariantOptionValueIds.IsNullOrEmpty())
        {
            validationMessages.Add(new(
                nameof(ProductVariantOptionValueIds),
                [$"The field {nameof(ProductVariantOptionValueIds)} is required."]
                ));
        }
    }
}
