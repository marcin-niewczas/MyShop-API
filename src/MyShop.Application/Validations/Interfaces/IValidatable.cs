using MyShop.Core.Validations;

namespace MyShop.Application.Validations.Interfaces;
public interface IValidatable
{
    void Validate(ICollection<ValidationMessage> validationMessages);
}
