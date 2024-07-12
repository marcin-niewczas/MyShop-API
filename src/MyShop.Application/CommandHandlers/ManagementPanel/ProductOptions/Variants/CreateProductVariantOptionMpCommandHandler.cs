using MyShop.Application.Commands.ManagementPanel.ProductOptions.Variants;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Products;
using MyShop.Core.Utils;

namespace MyShop.Application.CommandHandlers.ManagementPanel.ProductOptions.Variants;
internal sealed class CreateProductVariantOptionMpCommandHandler(
    IUnitOfWork unitOfWork
    ) : ICommandHandler<CreateProductVariantOptionMp, ApiIdResponse>
{
    public async Task<ApiIdResponse> HandleAsync(CreateProductVariantOptionMp command, CancellationToken cancellationToken = default)
    {
        var isExist = await unitOfWork.BaseProductOptionRepository.AnyAsync(
            e => Convert.ToString(e.Name).ToLower().Equals(command.Name.ToLower()),
            cancellationToken
            );

        if (isExist)
        {
            throw new BadRequestException(
                $"{nameof(ProductVariantOption).ToTitleCase()} with {nameof(ProductVariantOption.Name)} equal '{command.Name}' exist."
                );
        }

        var entity = new ProductVariantOption(
            command.Name,
            command.ProductOptionSubtype,
            command.ProductOptionSortType,
            command.ProductVariantOptionValues
            );

        await unitOfWork.ProductVariantOptionRepository.AddAsync(entity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new ApiIdResponse(entity.Id);
    }
}
