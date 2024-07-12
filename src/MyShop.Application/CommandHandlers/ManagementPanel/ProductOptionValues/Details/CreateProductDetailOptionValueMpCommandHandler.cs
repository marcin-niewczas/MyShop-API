using MyShop.Application.Commands.ManagementPanel.ProductOptionValues.Details;
using MyShop.Application.Dtos.ManagementPanel.ProductOptionValues;
using MyShop.Application.Mappings;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Products;
using MyShop.Core.Utils;

namespace MyShop.Application.CommandHandlers.ManagementPanel.ProductOptionValues.Details;
internal sealed class CreateProductDetailOptionValueMpCommandHandler(
    IUnitOfWork unitOfWork
    ) : ICommandHandler<CreateProductDetailOptionValueMp, ApiResponse<ProductOptionValueMpDto>>
{
    public async Task<ApiResponse<ProductOptionValueMpDto>> HandleAsync(CreateProductDetailOptionValueMp command, CancellationToken cancellationToken = default)
    {
        var productDetailOption = await unitOfWork.ProductDetailOptionRepository.GetByIdAsync(
             id: command.ProductDetailOptionId,
             includeExpression: i => i.ProductOptionValues.OrderBy(v => v.Position),
             withTracking: true,
             cancellationToken: cancellationToken
             ) ?? throw new NotFoundException(nameof(ProductVariantOption).ToTitleCase(), command.ProductDetailOptionId);

        var value = new ProductDetailOptionValue(command.Value, command.ProductDetailOptionId);

        productDetailOption.AddProductDetailOptionValue(value);

        await unitOfWork.ProductDetailOptionValueRepository.AddAsync(value, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new(value.ToProductOptionValueMpDto());
    }
}
