using MyShop.Application.Commands.ManagementPanel.Products;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.HelperModels;
using MyShop.Core.Models.Products;
using MyShop.Core.Utils;
using MyShop.Core.ValueObjects.Categories;
using MyShop.Core.ValueObjects.ProductOptions;

namespace MyShop.Application.CommandHandlers.ManagementPanel.Products;
internal sealed class CreateProductMpCommandHandler(
    IUnitOfWork unitOfWork
    ) : ICommandHandler<CreateProductMp, ApiIdResponse>
{
    public async Task<ApiIdResponse> HandleAsync(CreateProductMp command, CancellationToken cancellationToken = default)
    {
        var isExist = await unitOfWork.ProductRepository.AnyAsync(
            e => Convert.ToString(e.Name).ToLower().Equals(command.Name.ToLower()),
            cancellationToken
            );

        if (isExist)
            throw new BadRequestException($"The {nameof(Product)} with {nameof(Product.Name)} equal '{command.Name}' exist.");

        var category = await unitOfWork.CategoryRepository.GetFirstByPredicateAsync(e => e.Id == command.ProductCategoryId, cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"The {nameof(Category)}", command.ProductCategoryId);

        if (category.HierarchyDetail.Level != CategoryLevel.Max)
            throw new BadRequestException($"The {nameof(Category)} with id equal '{category.Id}' isn't {nameof(Product)} {nameof(Category)}.");

        if (command.ChosenProductDetailOptionValues.Count <= 0)
            throw new BadRequestException($"The {nameof(Product)} must have at least 1 {nameof(ProductDetailOptionValue)} with type {ProductOptionSubtype.Main}.");

        if (command.ChosenProductVariantOptions.Count <= 0)
            throw new BadRequestException($"The {nameof(Product)} must have at least 1 {nameof(ProductVariantOption)} with type {ProductOptionSubtype.Main}.");

        var productDetailOptionValues = await unitOfWork.ProductDetailOptionValueRepository.GetByPredicateAsync(
            e => command.ChosenProductDetailOptionValues.Select(c => c.Value).Contains(e.Id),
            includeExpression: i => i.ProductDetailOption,
            cancellationToken: cancellationToken
            );

        if (productDetailOptionValues.Count <= 0 || productDetailOptionValues.Count != command.ChosenProductDetailOptionValues.Count)
            throw new BadRequestException($"Invalid Chosen {nameof(ProductDetailOptionValue)} Ids.");

        var productVariantOptions = await unitOfWork.ProductVariantOptionRepository.GetByPredicateAsync(
            predicate: e => command.ChosenProductVariantOptions.Select(c => c.Value).Contains(e.Id),
            cancellationToken: cancellationToken
            );

        if (productVariantOptions.Count <= 0 || productVariantOptions.Count != command.ChosenProductVariantOptions.Count)
            throw new BadRequestException($"Invalid Chosen {nameof(ProductVariantOption).ToTitleCase()} Ids.");

        var valuePositionsOfProductDetailOptionValues = command.ChosenProductDetailOptionValues.Select(
            c => new ValuePosition<ProductDetailOptionValue>(productDetailOptionValues.First(v => v.Id == c.Value), c.Position)
            ).ToArray();

        var valuePositionsOfProductVariantOptions = command.ChosenProductVariantOptions.Select(
            c => new ValuePosition<ProductVariantOption>(productVariantOptions.First(v => v.Id == c.Value), c.Position)
            ).ToArray();

        var product = new Product(
            command.Name,
            command.DisplayProductPer,
            command.Description,
            command.ProductCategoryId,
            valuePositionsOfProductDetailOptionValues,
            valuePositionsOfProductVariantOptions
            );

        product = await unitOfWork.ProductRepository.AddAsync(product, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new ApiIdResponse(product.Id);
    }
}
