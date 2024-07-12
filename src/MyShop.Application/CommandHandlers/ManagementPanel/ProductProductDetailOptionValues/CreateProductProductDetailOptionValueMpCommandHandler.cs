using MyShop.Application.Commands.ManagementPanel.ProductProductDetailOptionValues;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Products;
using MyShop.Core.ValueObjects.ProductOptions;

namespace MyShop.Application.CommandHandlers.ManagementPanel.ProductProductDetailOptionValues;
internal sealed class CreateProductProductDetailOptionValueMpCommandHandler(
    IUnitOfWork unitOfWork
    ) : ICommandHandler<CreateProductProductDetailOptionValueMp>
{
    public async Task HandleAsync(CreateProductProductDetailOptionValueMp command, CancellationToken cancellationToken = default)
    {
        var productExist = await unitOfWork.ProductRepository.AnyAsync(
            predicate: e => e.Id == command.ProductId,
            cancellationToken: cancellationToken
            );

        if (!productExist)
        {
            throw new NotFoundException(nameof(Product), command.ProductId);
        }

        var value = await unitOfWork.ProductDetailOptionValueRepository.GetFirstByPredicateAsync(
            predicate: e => e.Id == command.ProductDetailOptionValueId,
            includeExpression: i => i.ProductDetailOption,
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException(nameof(ProductDetailOptionValue), command.ProductId);

        if (value.ProductDetailOption.ProductOptionSubtype != ProductOptionSubtype.Additional)
        {
            throw new BadRequestException($"The {nameof(ProductDetailOptionValue)} '{value.Id}' isn't {ProductOptionSubtype.Additional}.");
        }

        var isCorrectProduct = await unitOfWork.ProductRepository.AnyAsync(
            predicate: e => e.Id == command.ProductId && !e.ProductDetailOptionValues.Any(v => v.ProductOptionId == value.ProductOptionId),
            cancellationToken: cancellationToken
            );

        if (!isCorrectProduct)
        {
            throw new BadRequestException(
                $"The {nameof(Product)} can contain exactly one {nameof(ProductDetailOptionValue)} with associated {nameof(ProductDetailOption)}."
                );
        }

        var valuesCount = await unitOfWork.ProductProductDetailOptionValueRespository.CountAsync(
            predicate: e => e.ProductId == command.ProductId,
            cancellationToken: cancellationToken
            );

        var entity = new ProductProductDetailOptionValue(
            productId: command.ProductId,
            productDetailOptionValueId: command.ProductDetailOptionValueId,
            position: valuesCount
            );

        await unitOfWork.AddAsync(entity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
