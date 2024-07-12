using MyShop.Application.Commands.ManagementPanel.Categories;
using MyShop.Application.Mappings;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Dtos.ManagementPanel;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Products;

namespace MyShop.Application.CommandHandlers.ManagementPanel.Categories;
internal sealed class UpdateCategoryMpCommandHandler(
    IUnitOfWork unitOfWork
    ) : ICommandHandler<UpdateCategoryMp, ApiResponse<CategoryMpDto>>
{
    public async Task<ApiResponse<CategoryMpDto>> HandleAsync(UpdateCategoryMp command, CancellationToken cancellationToken = default)
    {
        var category = await unitOfWork.CategoryRepository.GetByIdAsync(command.Id, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(Category), command.Id);

        category = await (category.ParentCategoryId switch
        {
            Guid parentCategoryId => GetUpdatedChildrenCategoryAsync(category, command, parentCategoryId, cancellationToken),
            _ => GetUpdatedRootCategoryAsync(category, command, cancellationToken)
        });

        await unitOfWork.CategoryRepository.UpdateAsync(category);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new(category.ToCategoryMpDto());
    }

    private async Task<Category> GetUpdatedChildrenCategoryAsync(Category category, UpdateCategoryMp command, Guid parentCategoryId, CancellationToken cancellationToken = default)
    {
        category.UpdateChildrenCategory(command.Name);

        var parentCategory = await unitOfWork.CategoryRepository.GetByIdAsync(parentCategoryId, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(Category.ParentCategory), parentCategoryId);

        var isExist = await unitOfWork.CategoryRepository
            .AnyAsync(
                predicate: e => e.HierarchyDetail.RootCategoryId == parentCategory.HierarchyDetail.RootCategoryId && Convert.ToString(e.Name).ToLower().Equals(command.Name.ToLower()),
                cancellationToken: cancellationToken
                );

        if (isExist)
        {
            throw new BadRequestException($"The {nameof(Category)} with '{command.Name}' {nameof(Category.Name)} exist in structure.");
        }

        return category;
    }

    private async Task<Category> GetUpdatedRootCategoryAsync(Category category, UpdateCategoryMp command, CancellationToken cancellationToken = default)
    {
        await category.IncludeLowerCategoriesAsync(unitOfWork.CategoryRepository, cancellationToken);
        category.UpdateRootCategory(command.Name);

        var isExist = await unitOfWork.CategoryRepository
            .AnyAsync(
                e => (e.ParentCategoryId == null || e.HierarchyDetail.RootCategoryId == category.Id) && Convert.ToString(e.Name).ToLower().Equals(command.Name.ToLower()),
                cancellationToken
                );

        if (isExist)
        {
            throw new BadRequestException(
                $"The {nameof(Category)} with {nameof(Category.Name)} equals '{command.Name}' exist in structure or other Main {nameof(Category)} have the same {nameof(Category.Name)}."
                );
        }

        return category;
    }
}
