using MyShop.Application.Responses;
using MyShop.Application.Validations.Interfaces;
using MyShop.Core.Dtos.ManagementPanel;
using MyShop.Core.Validations;
using MyShop.Core.ValueObjects.Categories;

namespace MyShop.Application.Commands.ManagementPanel.Categories;
public sealed record UpdateCategoryMp(
    Guid Id,
    string Name
    ) : ICommand<ApiResponse<CategoryMpDto>>, IValidatable
{
    public void Validate(ICollection<ValidationMessage> validationMessages)
    {
        CategoryName.Validate(Name, validationMessages);
    }
}
