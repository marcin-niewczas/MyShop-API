using MyShop.Application.Commands.ManagementPanel.MainPageSections;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.BaseEntities;
using MyShop.Core.Models.MainPageSections;
using MyShop.Core.Utils;
using MyShop.Core.ValueObjects.MainPageSections;

namespace MyShop.Application.CommandHandlers.ManagementPanel.MainPageSections;
internal sealed class UpdatePositionsOfMainPageSectionsMpCommandHandler(
    IUnitOfWork unitOfWork
    ) : ICommandHandler<UpdatePositionsOfMainPageSectionsMp>
{
    public async Task HandleAsync(UpdatePositionsOfMainPageSectionsMp command, CancellationToken cancellationToken = default)
    {
        var sections = await unitOfWork.MainPageSectionRepository.GetAllAsync(
            cancellationToken: cancellationToken
            );

        if (sections.IsNullOrEmpty())
        {
            throw new NotFoundException(command.IdPositions.Count switch
            {
                > 1 => $"Not found {nameof(MainPageSections)} with introduced {nameof(IEntity.Id)}s.",
                _ => $"Not found {nameof(MainPageSection)} with introduced {nameof(IEntity.Id)}."
            });
        }

        var maxPosition = sections.Count - 1;

        if (!MainPageSectionPosition.IsValid(command.IdPositions.Max(c => c.Position)))
        {
            throw new BadRequestException($"Positions must be between {MainPageSectionPosition.Min} and {maxPosition}.");
        }

        foreach (var item in command.IdPositions)
        {
            (sections.FirstOrDefault(e => e.Id == item.Value)
                ?? throw new NotFoundException(nameof(MainPageSection), item.Value)
            ).UpdatePosition(item.Position);
        }

        var allowedPositions = Enumerable.Range(MainPageSectionPosition.Min, sections.Count);

        if (sections.HasDuplicateBy(e => e.Position) || sections.Any(e => !allowedPositions.Contains(e.Position)))
        {
            throw new BadRequestException($"Invalid position/positions. The positions must be unique and not contains gaps.");
        }

        await unitOfWork.UpdateAsync(sections);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
