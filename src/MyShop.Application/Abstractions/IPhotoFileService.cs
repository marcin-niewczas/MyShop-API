using Microsoft.AspNetCore.Http;
using MyShop.Core.HelperModels;

namespace MyShop.Application.Abstractions;
public interface IPhotoFileService
{
    Task<bool> DeletePhotoAsync(IEnumerable<string> photoFilePaths);
    Task<bool> DeletePhotoAsync(string photoFilePaths);
    Task<FileDetail> SavePhotoAsync(IFormFile formFile, string? photoName = null, int? position = null, CancellationToken cancellationToken = default);
    IAsyncEnumerable<FileDetail> SavePhotosAsync(IFormFileCollection formFiles, string? photoName = null, CancellationToken cancellationToken = default);
    IAsyncEnumerable<FileDetail> SaveProductVariantPhotosAsync(
        IFormFileCollection formFileCollection,
        string? photoName = null,
        CancellationToken cancellationToken = default
        );
}
