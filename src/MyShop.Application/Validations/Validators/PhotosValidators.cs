using Microsoft.AspNetCore.Http;
using MyShop.Core.Validations;
using MyShop.Core.ValueObjects.Photos;
using MyShop.Core.ValueObjects.Products;

namespace MyShop.Application.Validations.Validators;
public static partial class CustomValidators
{
    public static class Photos
    {
        public const string ProductVariantPhotosFormFileName = "productVariantPhotos";
        public const string UserPhotoFormFileName = "userPhoto";
        public const string WebsiteHeroSectionItemPhotoFormFileName = "websiteHeroSectionItemPhoto";

        public static IReadOnlyCollection<string> AllowedFormFileNames { get; } =
        [
            ProductVariantPhotosFormFileName,
            UserPhotoFormFileName,
            WebsiteHeroSectionItemPhotoFormFileName
        ];

        public static string ErrorMessage { get; }
            = $"The uploaded photos form names must be in [ {string.Join(", ", AllowedFormFileNames)} ].";

        public static void Validate(
           IFormFile? formFile,
           PhotoType photoType,
           ICollection<ValidationMessage> validationMessages,
           string paramName,
           bool isNullable = false
           )
        {
            if (isNullable && formFile is null)
            {
                return;
            }

            if (formFile is null)
            {
                validationMessages.Add(new(
                    paramName,
                    [$"The field {paramName} is required."]
                    ));

                return;
            }

            var correctFormFileName = GetCorrectFormFileName(photoType);

            if (correctFormFileName != formFile.Name)
            {
                validationMessages.Add(new(
                    paramName,
                    [$"The field {paramName} must have a {nameof(FormFile)}Name equals to '{correctFormFileName}'."]
                    ));
            }

            Enums.MustBeIn<PhotoContentType>(formFile.ContentType, validationMessages, paramName);

            if (formFile.FileName is not null)
            {
                var splittedFileName = formFile.FileName.Split('.');

                if (splittedFileName.Length > 1)
                {
                    Enums.MustBeIn<PhotoExtension>($".{splittedFileName.Last()}", validationMessages, paramName);

                    if (correctFormFileName == ProductVariantPhotosFormFileName)
                    {
                        if (int.TryParse(splittedFileName[0], out var pos))
                        {
                            if (!ProductVariantPhotoItemPosition.IsValid(pos))
                            {
                                validationMessages.Add(new(
                                paramName,
                                [$"The file name '{formFile.FileName}' is incorrect. " +
                                 $"It has out of range position in name. The Position should be inclusive between {ProductVariantPhotoItemPosition.Min} and {ProductVariantPhotoItemPosition.Max}."]
                                ));
                            }
                        }
                        else
                        {
                            validationMessages.Add(new(
                                paramName,
                                [$"The file name '{formFile.FileName}' is incorrect. It must have only position and extension in name. For example 1.jpg."]
                                ));
                        }
                    }
                }
                else
                {
                    validationMessages.Add(new(
                        paramName,
                        [$"The File have a incorrect File Name."]
                        ));
                }
            }
            else
            {
                validationMessages.Add(new(
                    paramName,
                    [$"The File must have a File Name."]
                    ));
            }

            PhotoSize.Validate(formFile.Length / 1024m, validationMessages);
        }

        private static string GetCorrectFormFileName(
            PhotoType photoType
            ) => photoType.Value switch
            {
                PhotoType.ProductVariantPhoto => ProductVariantPhotosFormFileName,
                PhotoType.WebsiteHeroPhoto => WebsiteHeroSectionItemPhotoFormFileName,
                PhotoType.UserPhoto => UserPhotoFormFileName,
                _ => throw new NotImplementedException()
            };
    }
}
