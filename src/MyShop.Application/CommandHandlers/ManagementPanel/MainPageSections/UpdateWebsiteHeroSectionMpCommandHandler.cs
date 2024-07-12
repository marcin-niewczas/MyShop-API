using MyShop.Application.Commands.ManagementPanel.MainPageSections;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.MainPageSections;

namespace MyShop.Application.CommandHandlers.ManagementPanel.MainPageSections;
internal sealed class UpdateWebsiteHeroSectionMpCommandHandler(
    IUnitOfWork unitOfWork
    ) : ICommandHandler<UpdateWebsiteHeroSectionMp>
{
    public async Task HandleAsync(
        UpdateWebsiteHeroSectionMp command,
        CancellationToken cancellationToken = default
        )
    {
        var entity = await unitOfWork.WebsiteHeroSectionRepository.GetByIdAsync(
            id: command.Id,
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException(nameof(WebsiteHeroSection), command.Id);

        entity.Update(command.Label);

        await unitOfWork.UpdateAsync(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
