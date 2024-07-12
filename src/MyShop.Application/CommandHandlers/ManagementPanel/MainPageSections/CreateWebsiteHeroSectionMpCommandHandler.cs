using MyShop.Application.Commands.ManagementPanel.MainPageSections;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.MainPageSections;
using MyShop.Core.ValueObjects.MainPageSections;

namespace MyShop.Application.CommandHandlers.ManagementPanel.MainPageSections;
internal sealed class CreateWebsiteHeroSectionMpCommandHandler(
    IUnitOfWork unitOfWork
    ) : ICommandHandler<CreateWebsiteHeroSectionMp, ApiIdResponse>
{
    public async Task<ApiIdResponse> HandleAsync(CreateWebsiteHeroSectionMp command, CancellationToken cancellationToken = default)
    {
        var isExist = await unitOfWork.WebsiteHeroSectionRepository.AnyAsync(
            predicate: e => Convert.ToString(e.Label).ToLower().Equals(command.Label.ToLower()),
            cancellationToken: cancellationToken
            );

        if (isExist)
        {
            throw new BadRequestException(
                $"The {nameof(WebsiteHeroSection)} with {nameof(WebsiteHeroSection.Label)} equal '{command.Label}' exist."
                );
        }

        var count = await unitOfWork.MainPageSectionRepository.CountAsync(cancellationToken);

        if (!MainPageSectionPosition.IsValid(count))
        {
            throw new BadRequestException(
                $"Cannot create {nameof(WebsiteHeroSection)}, because max. limit of {nameof(MainPageSection)}s are equal to {MainPageSectionPosition.Max + 1}."
                );
        }

        var maxPosition = await unitOfWork.MainPageSectionRepository.MaxAsync(e => e.Position, cancellationToken);

        var entity = new WebsiteHeroSection(
            command.Label,
            command.DisplayType,
            maxPosition is null ? 0 : maxPosition + 1
            );

        await unitOfWork.AddAsync(entity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new(entity.Id);
    }
}
