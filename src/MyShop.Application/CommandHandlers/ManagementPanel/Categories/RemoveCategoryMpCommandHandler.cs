using MyShop.Application.Commands.ManagementPanel.Categories;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Products;
using MyShop.Core.ValueObjects.Categories;

namespace MyShop.Application.CommandHandlers.ManagementPanel.Categories;
internal sealed class RemoveCategoryMpCommandHandler(
    IUnitOfWork unitOfWork
    ) : ICommandHandler<RemoveCategoryMp>
{
    public async Task HandleAsync(RemoveCategoryMp command, CancellationToken cancellationToken = default)
    {
        var category = await unitOfWork.CategoryRepository.GetByIdAsync(
            id: command.Id,
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException(nameof(Category), command.Id);

        if (category.HierarchyDetail.Level >= CategoryLevel.Max)
        {
            var countProductsUseThisCategory = await unitOfWork.ProductRepository.CountAsync(
                e => e.CategoryId == category.Id,
                cancellationToken
                );

            if (countProductsUseThisCategory > 0)
            {
                throw new BadRequestException(
                    $"Cannot remove {nameof(Category)} '{category.HierarchyDetail.HierarchyName}', beacuse {(countProductsUseThisCategory > 1) switch
                    {
                        true => $"{countProductsUseThisCategory} {nameof(Product)}s use",
                        _ => $"{countProductsUseThisCategory} {nameof(Product)} uses"
                    }} this {nameof(Category)},"
                    );
            }
        }
        else
        {
            var countChildrenOfThisCategory = await unitOfWork.CategoryRepository.CountAsync(
                e => e.ParentCategoryId == category.Id,
                cancellationToken
                );

            if (countChildrenOfThisCategory > 0)
            {
                throw new BadRequestException(
                    $"Cannot remove {nameof(Category)} '{category.HierarchyDetail.HierarchyName}', beacuse it has {countChildrenOfThisCategory} {(countChildrenOfThisCategory > 1) switch
                    {
                        true => $"Child Categories",
                        _ => $"Child {nameof(Category)}"
                    }}."
                    );
            }
        }

        await unitOfWork.RemoveAsync(category);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
