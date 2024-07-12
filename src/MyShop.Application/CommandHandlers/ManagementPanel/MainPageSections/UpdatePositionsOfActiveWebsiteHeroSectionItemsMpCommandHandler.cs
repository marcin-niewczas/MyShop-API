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
internal sealed class UpdatePositionsOfActiveWebsiteHeroSectionItemsMpCommandHandler(
    IUnitOfWork unitOfWork
    ) : ICommandHandler<UpdatePositionsOfActiveWebsiteHeroSectionItemsMp>
{
    public async Task HandleAsync(
        UpdatePositionsOfActiveWebsiteHeroSectionItemsMp command,
        CancellationToken cancellationToken = default
        )
    {
        var websiteHeroSection = await unitOfWork.WebsiteHeroSectionRepository.GetByIdAsync(
            id: command.WebsiteHeroSectionId,
            includeExpression: i => i.WebsiteHeroSectionItems,
            withTracking: true,
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException(nameof(WebsiteHeroSection), command.WebsiteHeroSectionId);

        websiteHeroSection.UpdatePositionsOfWebsiteHeroSectionItems(command.IdPositions);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
