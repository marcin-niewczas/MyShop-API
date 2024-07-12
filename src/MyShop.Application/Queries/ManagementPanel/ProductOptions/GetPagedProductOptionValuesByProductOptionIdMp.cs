using MyShop.Application.Dtos.ManagementPanel.ProductOptionValues;
using MyShop.Application.EndpointQueries.Interfaces;
using MyShop.Application.Responses;
using MyShop.Application.Validations.Interfaces;
using MyShop.Application.Validations.Validators;
using MyShop.Core.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Application.Queries.ManagementPanel.ProductOptions;
public sealed record GetPagedProductOptionValuesByProductOptionIdMp(
    Guid Id,
    int PageNumber,
    int PageSize,
    string? SearchPhrase
    ) : IQuery<ApiPagedResponse<ProductOptionValueMpDto>>,
        IPaginationQueryParams,
        ISearchQueryParams,
        IValidatable
{
    public void Validate(ICollection<ValidationMessage> validationMessages)
    {
        CustomValidators.PaginationParams.Validate(PageNumber, PageSize, validationMessages);
    }
}
