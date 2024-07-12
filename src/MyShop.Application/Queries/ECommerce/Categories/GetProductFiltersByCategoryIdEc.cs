using Microsoft.AspNetCore.Mvc;
using MyShop.Application.Dtos.ECommerce.Products;
using MyShop.Application.EndpointQueries.ProductOptions;
using MyShop.Application.Responses;
using MyShop.Application.Validations.Interfaces;
using MyShop.Application.Validations.Validators;
using MyShop.Core.Validations;

namespace MyShop.Application.Queries.ECommerce.Categories;
public sealed record GetProductFiltersByCategoryIdEc(
    [FromRoute] string EncodedCategoryName,
    [FromQuery] decimal? MinPrice,
    [FromQuery] decimal? MaxPrice,
    [FromQuery] ProductOptionParam? ProductOptionParam
    ) : IQuery<ApiResponse<ProductFiltersDtoEc>>,
        IValidatable
{
    public void Validate(ICollection<ValidationMessage> validationMessages)
    {
        CustomValidators.Numbers.MustBeNullOrPositive(MinPrice, validationMessages, nameof(MinPrice));
        CustomValidators.Numbers.MustBeNullOrPositive(MaxPrice, validationMessages, nameof(MaxPrice));
    }
}
