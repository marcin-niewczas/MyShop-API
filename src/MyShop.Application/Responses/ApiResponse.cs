using MyShop.Core.Abstractions;

namespace MyShop.Application.Responses;
public sealed record ApiResponse<TDto>(
    TDto Data
    ) : IApiResponse where TDto : class, IDto;
