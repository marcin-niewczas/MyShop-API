using MyShop.Core.Models.BaseEntities;
using MyShop.Core.Models.Users;

namespace MyShop.Core.Models.Products;
public sealed class Favorite : BaseTimestampEntity
{
    public string EncodedProductVariantName { get; set; }
    public RegisteredUser RegisteredUser { get; set; } = default!;
    public Guid RegisteredUserId { get; set; }

    public Favorite(string encodedProductVariantName, Guid registeredUserId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(encodedProductVariantName, nameof(encodedProductVariantName));

        EncodedProductVariantName = encodedProductVariantName;
        RegisteredUserId = registeredUserId;
    }
}
