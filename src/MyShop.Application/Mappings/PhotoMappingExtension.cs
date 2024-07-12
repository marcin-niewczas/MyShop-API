using MyShop.Application.Dtos.ManagementPanel.Photos;
using MyShop.Core.Models.Photos;

namespace MyShop.Application.Mappings;
internal static class PhotoMappingExtension
{
    public static PhotoMpDto ToPhotoMpDto(this Photo entity)
        => new()
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            Name = entity.Name,
            PhotoExtension = entity.Extension,
            ContentType = entity.ContentType,
            PhotoSize = entity.PhotoSize,
            Url = entity.Uri.ToString(),
            Alt = entity.Alt,
            PhotoType = entity.PhotoType,
        };

    public static IReadOnlyCollection<PhotoMpDto> ToPhotoMpDtos(this IEnumerable<Photo> entities)
        => entities.Select(ToPhotoMpDto).ToArray();
}
