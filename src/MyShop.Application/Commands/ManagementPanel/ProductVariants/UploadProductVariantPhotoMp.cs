using Microsoft.AspNetCore.Http;
using MyShop.Application.Validations.Interfaces;
using MyShop.Application.Validations.Validators;
using MyShop.Core.Utils;
using MyShop.Core.Validations;
using MyShop.Core.ValueObjects.Photos;
using MyShop.Core.ValueObjects.Products;

namespace MyShop.Application.Commands.ManagementPanel.ProductVariants;
public sealed record UploadProductVariantPhotoMp(
    Guid Id,
    IFormFileCollection ProductVariantPhotos
    ) : ICommand,
        IValidatable
{
    public void Validate(ICollection<ValidationMessage> validationMessages)
    {
        if (ProductVariantPhotos.IsNullOrEmpty())
        {
            validationMessages.Add(
                new(
                    nameof(ProductVariantPhotos),
                    [$"The field {nameof(ProductVariantPhotos)} is required."]
                    )
                );

            return;
        }

        var maxCount = ProductVariantPhotoItemPosition.Max + 1;

        if (ProductVariantPhotos.Count > maxCount)
        {
            validationMessages.Add(
                new(
                    nameof(ProductVariantPhotos),
                    [$"The collection {nameof(ProductVariantPhotos)} can have max. {maxCount} items."]
                    )
                );

            return;
        }

        foreach (var photo in ProductVariantPhotos)
        {
            CustomValidators.Photos.Validate(
                photo,
                PhotoType.ProductVariantPhoto,
                validationMessages,
                nameof(ProductVariantPhotos)
                );
        }
    }
}
