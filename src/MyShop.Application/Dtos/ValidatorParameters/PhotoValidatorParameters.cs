using MyShop.Core.ValueObjects.Photos;

namespace MyShop.Application.Dtos.ValidatorParameters;
public sealed record PhotoValidatorParameters
{
    public IReadOnlyCollection<string> AllowedContentTypes { get; }
        = PhotoContentType.AllowedValues.Cast<string>().ToArray();
    public IReadOnlyCollection<string> AllowedPhotoExtensions { get; }
        = PhotoExtension.AllowedValues.Cast<string>().ToArray();
    public int MaxSizeInMegabytes { get; }
        = PhotoSize.MaxSizeInMegabytes;
    public bool Multiple { get; init; } = false;
    public uint MaxPhotosCount { get; init; } = 1;
}
