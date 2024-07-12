using MyShop.Core.Abstractions;
using MyShop.Core.Models.BaseEntities;

namespace MyShop.Core.Dtos.Account;
public sealed record OrderAcDto : BaseTimestampDto, IModel
{
    public required decimal TotalPrice { get; init; }
    public required string Status { get; init; }
}
