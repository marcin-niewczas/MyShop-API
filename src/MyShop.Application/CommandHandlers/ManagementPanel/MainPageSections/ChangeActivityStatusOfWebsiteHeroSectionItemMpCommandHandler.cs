using MyShop.Application.Commands.ManagementPanel.MainPageSections;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.MainPageSections;

namespace MyShop.Application.CommandHandlers.ManagementPanel.MainPageSections;
internal sealed class ChangeActivityStatusOfWebsiteHeroSectionItemMpCommandHandler(
    IUnitOfWork unitOfWork
    ) : ICommandHandler<ChangeActivityStatusOfWebsiteHeroSectionItemMp>
{
    public async Task HandleAsync(
        ChangeActivityStatusOfWebsiteHeroSectionItemMp command,
        CancellationToken cancellationToken = default
        )
    {
        var websiteHeroSection = await unitOfWork.WebsiteHeroSectionRepository.GetFirstByPredicateAsync(
                predicate: e => e.WebsiteHeroSectionItems.Any(i => i.Id == command.Id),
                includeExpression: i => i.WebsiteHeroSectionItems.Where(e => e.Position != null || e.Id == command.Id).OrderBy(o => o.Position),
                withTracking: true,
                cancellationToken: cancellationToken
                ) ?? throw new NotFoundException(nameof(WebsiteHeroSection), command.Id);

        if (command.Active)
        {
            websiteHeroSection.ActivateItem(command.Id);
        }
        else
        {
            websiteHeroSection.DeactivationItem(command.Id);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
