using MyShop.Core.Abstractions;

namespace MyShop.Application.Dtos;
public sealed record ValueDto<TValue>(
    TValue Value
    ) : IDto;
