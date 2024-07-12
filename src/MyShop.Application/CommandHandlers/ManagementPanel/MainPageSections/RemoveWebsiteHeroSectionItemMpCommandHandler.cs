using MyShop.Application.Commands.ManagementPanel.MainPageSections;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.MainPageSections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Application.CommandHandlers.ManagementPanel.MainPageSections;
internal sealed class RemoveWebsiteHeroSectionItemMpCommandHandler(
    IUnitOfWork unitOfWork
    ) : ICommandHandler<RemoveWebsiteHeroSectionItemMp>
{
    public async Task HandleAsync(RemoveWebsiteHeroSectionItemMp command, CancellationToken cancellationToken = default)
    {
        var websiteHeroSection = await unitOfWork.WebsiteHeroSectionRepository.GetFirstByPredicateAsync(
                predicate: e => e.WebsiteHeroSectionItems.Any(i => i.Id == command.Id),
                includeExpression: i => i.WebsiteHeroSectionItems.Where(e => e.Position != null || e.Id == command.Id).OrderBy(o => o.Position),
                withTracking: true,
                cancellationToken: cancellationToken
                ) ?? throw new NotFoundException(nameof(WebsiteHeroSection), command.Id);

        websiteHeroSection.RemoveItem(command.Id);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
