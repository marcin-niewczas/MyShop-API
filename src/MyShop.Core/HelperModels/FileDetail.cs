using MyShop.Core.ValueObjects.Photos;

namespace MyShop.Core.HelperModels;
public record FileDetail(
      string FileName,
      string FileExtension,
      string ContentType,
      decimal FileSize,
      string FilePath,
      Uri Uri,
      PhotoType PhotoType,
      int? Position = null
    );
