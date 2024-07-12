namespace MyShop.Application.Commands.ManagementPanel.MainPageSections;
public sealed record ChangeActivityStatusOfWebsiteHeroSectionItemMp(
    Guid Id,
    bool Active
    ) : ICommand;
