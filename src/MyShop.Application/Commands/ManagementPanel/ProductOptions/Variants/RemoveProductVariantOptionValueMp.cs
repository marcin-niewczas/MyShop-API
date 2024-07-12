namespace MyShop.Application.Commands.ManagementPanel.ProductOptions.Variants;
public sealed record RemoveProductVariantOptionValueMp(
    Guid Id
    ) : ICommand;
