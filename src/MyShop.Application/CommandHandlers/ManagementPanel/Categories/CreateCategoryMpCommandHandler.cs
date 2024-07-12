using MyShop.Application.Commands.ManagementPanel.Categories;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Products;

namespace MyShop.Application.CommandHandlers.ManagementPanel.Categories;
internal sealed class CreateCategoryMpCommandHandler(
    IUnitOfWork unitOfWork
    ) : ICommandHandler<CreateCategoryMp, ApiIdResponse>
{
    public async Task<ApiIdResponse> HandleAsync(CreateCategoryMp command, CancellationToken cancellationToken = default)
    {
        var entity = await (command.ParentCategoryId switch
        {
            Guid guid => BuildChildCategoryAsync(command, guid, cancellationToken),
            _ => BuildRootCategoryAsync(command, cancellationToken)
        });

        await unitOfWork.CategoryRepository.AddAsync(entity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new ApiIdResponse(entity.Id);
    }

    private async Task<Category> BuildChildCategoryAsync(CreateCategoryMp command, Guid parentCategoryId, CancellationToken cancellationToken = default)
    {
        var parentCategory = await unitOfWork.CategoryRepository.GetByIdAsync(parentCategoryId, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(Category.ParentCategory), parentCategoryId);

        var isExist = await unitOfWork.CategoryRepository
            .AnyAsync(
                predicate: e => e.HierarchyDetail.RootCategoryId == parentCategory.HierarchyDetail.RootCategoryId && Convert.ToString(e.Name).ToLower() == command.Name.ToLower(),
                cancellationToken: cancellationToken
                );

        if (isExist)
        {
            throw new BadRequestException($"The {nameof(Category)} with '{command.Name}' {nameof(Category.Name)} exist in structure.");
        }

        return new(command.Name, parentCategory);
    }

    private async Task<Category> BuildRootCategoryAsync(CreateCategoryMp command, CancellationToken cancellationToken = default)
    {
        var isExist = await unitOfWork.CategoryRepository
            .AnyAsync(
                e => Convert.ToString(e.Name).ToLower().Equals(command.Name.ToLower()) && e.ParentCategoryId == null,
                cancellationToken
            );

        if (isExist)
        {
            throw new BadRequestException($"Main {nameof(Category)} with {nameof(Category.Name)} equals '{command.Name}' exist.");
        }

        return new(command.Name);
    }
}
