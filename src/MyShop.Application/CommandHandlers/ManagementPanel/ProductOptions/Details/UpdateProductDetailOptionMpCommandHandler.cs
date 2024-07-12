using MyShop.Application.Commands.ManagementPanel.ProductOptions.Details;
using MyShop.Application.Dtos.ManagementPanel.ProductOptions;
using MyShop.Application.Mappings;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Products;
using MyShop.Core.Utils;
using MyShop.Core.ValueObjects.ProductOptions;

namespace MyShop.Application.CommandHandlers.ManagementPanel.ProductOptions.Details;
internal sealed class UpdateProductDetailOptionMpCommandHandler(
    IUnitOfWork unitOfWork
        ) : ICommandHandler<UpdateProductDetailOptionMp, ApiResponse<ProductOptionMpDto>>
{
    public async Task<ApiResponse<ProductOptionMpDto>> HandleAsync(UpdateProductDetailOptionMp command, CancellationToken cancellationToken = default)
    {
        var entity = await (command.ProductOptionSortType switch
        {
            ProductOptionSortType.Alphabetically => unitOfWork.ProductDetailOptionRepository.GetByIdAsync(
                id: command.Id,
                includeExpression: i => i.ProductOptionValues,
                cancellationToken: cancellationToken
                ),
            ProductOptionSortType.Custom => unitOfWork.ProductDetailOptionRepository.GetByIdAsync(
                id: command.Id,
                cancellationToken: cancellationToken
                ),
            _ => throw new BadRequestException(AllowedValuesError.Message<ProductOptionSortType>()),
        }) ?? throw new NotFoundException(nameof(ProductVariantOption).ToTitleCase(), command.Id);

        var isExist = await unitOfWork.BaseProductOptionRepository.AnyAsync(
            e => e.Id != command.Id && Convert.ToString(e.Name).ToLower().Equals(command.Name.ToLower()),
            cancellationToken
            );

        if (isExist)
        {
            throw new BadRequestException(
                $"{nameof(ProductDetailOption).ToTitleCase()} with {nameof(ProductDetailOption.Name)} equal {command.Name} exist."
                );
        }

        entity.Update(command.Name, command.ProductOptionSortType);
        await unitOfWork.ProductDetailOptionRepository.UpdateAsync(entity);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new(entity.ToProductOptionMpDto());
    }
}
