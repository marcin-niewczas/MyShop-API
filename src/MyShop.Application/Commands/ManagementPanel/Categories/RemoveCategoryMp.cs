namespace MyShop.Application.Commands.ManagementPanel.Categories;
public sealed record RemoveCategoryMp(
    Guid Id
    ) : ICommand;
