namespace MyShop.Application.Commands.ManagementPanel.ProductProductDetailOptionValues;
public sealed record CreateProductProductDetailOptionValueMp(
    Guid ProductId,
    Guid ProductDetailOptionValueId
    ) : ICommand;
