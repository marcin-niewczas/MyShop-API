using MyShop.Application.Responses;
using MyShop.Application.Validations.Interfaces;
using MyShop.Core.Validations;
using MyShop.Core.ValueObjects.Categories;

namespace MyShop.Application.Commands.ManagementPanel.Categories;
public sealed record CreateCategoryMp(
    string Name,
    Guid? ParentCategoryId
    ) : ICommand<ApiIdResponse>, 
        IValidatable
{
    public void Validate(ICollection<ValidationMessage> validationMessages)
    {
        CategoryName.Validate(Name, validationMessages);
    }
}
