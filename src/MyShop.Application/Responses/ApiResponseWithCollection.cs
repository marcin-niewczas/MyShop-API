using MyShop.Core.Abstractions;

namespace MyShop.Application.Responses;
public sealed record ApiResponseWithCollection<TDto>(
    IReadOnlyCollection<TDto> Data
    ) where TDto : class, IDto;
