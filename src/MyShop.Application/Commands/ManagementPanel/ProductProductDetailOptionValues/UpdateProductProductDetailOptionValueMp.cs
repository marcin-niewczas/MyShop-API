namespace MyShop.Application.Commands.ManagementPanel.ProductProductDetailOptionValues;
public sealed record UpdateProductProductDetailOptionValueMp(
    Guid Id,
    Guid ProductDetailOptionValueId
    ) : ICommand;
