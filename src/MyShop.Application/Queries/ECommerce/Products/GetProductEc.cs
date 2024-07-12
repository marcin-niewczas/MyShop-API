using MyShop.Application.Responses;
using MyShop.Application.Validations.Interfaces;
using MyShop.Core.Dtos.ECommerce;
using MyShop.Core.Validations;

namespace MyShop.Application.Queries.ECommerce.Products;
public sealed record GetProductEc(
    string EncodedName
    ) : IQuery<ApiResponse<BaseProductDetailEcDto>>,
        IValidatable
{
    public void Validate(ICollection<ValidationMessage> validationMessages)
    {
        if (string.IsNullOrWhiteSpace(EncodedName))
        {
            validationMessages.Add(new(
                nameof(EncodedName),
                [$"The field {nameof(EncodedName)} is required."]
                ));
        }
    }
}
