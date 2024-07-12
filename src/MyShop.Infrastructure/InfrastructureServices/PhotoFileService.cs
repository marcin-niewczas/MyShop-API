using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MyShop.Application.Abstractions;
using MyShop.Core.Exceptions;
using MyShop.Core.HelperModels;
using MyShop.Core.ValueObjects.Photos;
using MyShop.Core.ValueObjects.Products;
using System.Runtime.CompilerServices;
using PhotosValidators = MyShop.Application.Validations.Validators.CustomValidators.Photos;

namespace MyShop.Infrastructure.InfrastructureServices;
public sealed class PhotoFileService(
    IHttpContextAccessor httpContextAccessor,
    IWebHostEnvironment webHostEnvironment
    ) : IPhotoFileService
{
    private readonly HttpContext? _httpContext = httpContextAccessor?.HttpContext;

    private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

    public async Task<FileDetail> SavePhotoAsync(
        IFormFile formFile,
        string? photoName = null,
        int? position = null,
        CancellationToken cancellationToken = default
        )
    {
        var photoDestination = MapToPhotoType(formFile.Name);
        var fileExtension = Path.GetExtension(formFile.FileName);
        var uniqueFileName = GetUniquePhotoFileName(fileExtension, photoName);

        var photoUri = GetPhotoUri(photoDestination, uniqueFileName);

        var photoFilePath = GetFilePhotoPath(photoDestination, uniqueFileName);

        using var fileStream = File.Create(photoFilePath);
        await formFile.CopyToAsync(fileStream, cancellationToken);

        return new FileDetail(
            FileName: uniqueFileName,
            FileExtension: fileExtension,
            ContentType: formFile.ContentType,
            FileSize: GetFileSizeInKilobytes(formFile.Length),
            FilePath: photoFilePath,
            Uri: photoUri,
            PhotoType: photoDestination,
            Position: position
            );
    }

    public async IAsyncEnumerable<FileDetail> SavePhotosAsync(
        IFormFileCollection formFiles,
        string? photoName = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default
        )
    {
        foreach (var formFile in formFiles)
        {
            yield return await SavePhotoAsync(formFile, photoName, cancellationToken: cancellationToken);
        }
    }

    public async IAsyncEnumerable<FileDetail> SaveProductVariantPhotosAsync(
        IFormFileCollection formFileCollection,
        string? photoName = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default
        )
    {
        foreach (var formFile in formFileCollection)
        {
            var position = new ProductVariantPhotoItemPosition(
                int.Parse(formFile.FileName.Split('.')[0])
                );

            yield return await SavePhotoAsync(
                formFile,
                photoName: photoName,
                position: position,
                cancellationToken: cancellationToken
                );
        }
    }

    public Task<bool> DeletePhotoAsync(string photoFilePath)
    {
        if (!string.IsNullOrWhiteSpace(photoFilePath) && File.Exists(photoFilePath))
        {
            File.Delete(photoFilePath);
            return Task.FromResult(true);
        }

        return Task.FromResult(false);
    }

    public async Task<bool> DeletePhotoAsync(IEnumerable<string> photoFilePaths)
        => !(await Task.WhenAll(photoFilePaths.Select(DeletePhotoAsync))).Any(p => p is false);

    private static string GetUniquePhotoFileName(
        string fileExtension,
        string? photoName = null
        ) => photoName switch
        {
            null => $"{Path.GetRandomFileName()}-{Guid.NewGuid()}{fileExtension}",
            _ => $"{photoName}-{Guid.NewGuid()}{fileExtension}"
        };

    private string GetFilePhotoPath(PhotoType photoType, string uniquePhotoFileName)
        => Path.Combine(
            _webHostEnvironment.WebRootPath,
            photoType.Value switch
            {
                PhotoType.UserPhoto => Path.Combine("photos", "users", uniquePhotoFileName),
                PhotoType.ProductVariantPhoto => Path.Combine("photos", "product-variants", uniquePhotoFileName),
                PhotoType.WebsiteHeroPhoto => Path.Combine("photos", "website-sections", uniquePhotoFileName),
                _ => throw new NotSupportedException(AllowedValuesError.Message<PhotoType>())
            }
        );

    private Uri GetPhotoUri(PhotoType photoType, string uniquePhotoFileName)
    {
        if (_httpContext is null)
        {
            throw new InvalidOperationException(nameof(_httpContext));
        }

        var basePhotosUrl = $"{_httpContext.Request.Scheme}://{_httpContext.Request.Host.Value}/photos";

        return new Uri(photoType.Value switch
        {
            PhotoType.UserPhoto => $"{basePhotosUrl}/users/{uniquePhotoFileName}",
            PhotoType.ProductVariantPhoto => $"{basePhotosUrl}/product-variants/{uniquePhotoFileName}",
            PhotoType.WebsiteHeroPhoto => $"{basePhotosUrl}/website-sections/{uniquePhotoFileName}",
            _ => throw new NotSupportedException(AllowedValuesError.Message<PhotoType>())
        });
    }

    private static decimal GetFileSizeInKilobytes(long lengthInBytes)
        => Math.Round(lengthInBytes / 1024.0m);

    private static PhotoType MapToPhotoType(string name)
       => name switch
       {
           PhotosValidators.ProductVariantPhotosFormFileName => PhotoType.ProductVariantPhoto,
           PhotosValidators.UserPhotoFormFileName => PhotoType.UserPhoto,
           PhotosValidators.WebsiteHeroSectionItemPhotoFormFileName => PhotoType.WebsiteHeroPhoto,
           _ => throw new NotImplementedException(PhotosValidators.ErrorMessage)
       };
}
