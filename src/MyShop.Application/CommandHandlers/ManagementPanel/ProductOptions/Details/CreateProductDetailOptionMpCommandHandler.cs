using MyShop.Application.Commands.ManagementPanel.ProductOptions.Details;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Products;
using MyShop.Core.Utils;

namespace MyShop.Application.CommandHandlers.ManagementPanel.ProductOptions.Details;
internal sealed class CreateProductDetailOptionMpCommandHandler(
    IUnitOfWork unitOfWork
    ) : ICommandHandler<CreateProductDetailOptionMp, ApiIdResponse>
{
    public async Task<ApiIdResponse> HandleAsync(CreateProductDetailOptionMp command, CancellationToken cancellationToken = default)
    {
        var isExist = await unitOfWork.BaseProductOptionRepository.AnyAsync(
            e => Convert.ToString(e.Name).ToLower().Equals(command.Name.ToLower()),
            cancellationToken
            );

        if (isExist)
        {
            throw new BadRequestException(
                $"{nameof(ProductDetailOption).ToTitleCase()} with {nameof(ProductDetailOption.Name)} equal {command.Name} exist."
                );
        }

        var entity = new ProductDetailOption(
            command.Name,
            command.ProductOptionSubtype,
            command.ProductOptionSortType,
            command.ProductDetailOptionValues
            );

        await unitOfWork.ProductDetailOptionRepository.AddAsync(entity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new ApiIdResponse(entity.Id);
    }
}
