using MyShop.Core.Abstractions;

namespace MyShop.Core.Dtos.Shared;
public sealed record PhotoDto(
    Uri Url,
    string Alt
    ) : IDto;
