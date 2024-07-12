using MyShop.Application.Commands.ManagementPanel.MainPageSections;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.MainPageSections;
using MyShop.Core.Utils;
using MyShop.Core.ValueObjects.MainPageSections;

namespace MyShop.Application.CommandHandlers.ManagementPanel.MainPageSections;
internal sealed class CreateWebsiteProductCarouselSectionMpCommandHandler(
    IUnitOfWork unitOfWork
    ) : ICommandHandler<CreateWebsiteProductCarouselSectionMp, ApiIdResponse>
{
    public async Task<ApiIdResponse> HandleAsync(CreateWebsiteProductCarouselSectionMp command, CancellationToken cancellationToken = default)
    {
        var isExist = await unitOfWork.WebsiteProductCarouselSectionRepository.AnyAsync(
            predicate: e => e.ProductsCarouselSectionType == command.ProductsCarouselSectionType,
            cancellationToken: cancellationToken
            );

        if (isExist)
        {
            throw new BadRequestException($"The {nameof(WebsiteProductsCarouselSection).ToTitleCase()} with '{command.ProductsCarouselSectionType}' exist.");
        }

        var count = await unitOfWork.MainPageSectionRepository.CountAsync(cancellationToken);

        if (!MainPageSectionPosition.IsValid(count))
        {
            throw new BadRequestException(
                $"Cannot create {nameof(WebsiteProductsCarouselSection)}, because max. limit of {nameof(MainPageSection)}s are equal to {MainPageSectionPosition.Max + 1}."
                );
        }

        var maxPosition = await unitOfWork.MainPageSectionRepository.MaxAsync(e => e.Position, cancellationToken);

        var entity = new WebsiteProductsCarouselSection(command.ProductsCarouselSectionType, maxPosition is null ? 0 : maxPosition + 1);

        await unitOfWork.AddAsync(entity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new(entity.Id);
    }
}
