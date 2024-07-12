using MyShop.Application.Dtos.Account.Users;
using MyShop.Application.EndpointQueries.Interfaces;
using MyShop.Application.Responses;
using MyShop.Application.Validations.Interfaces;
using MyShop.Application.Validations.Validators;
using MyShop.Core.Validations;

namespace MyShop.Application.Queries.Account.Users;
public sealed record GetPagedUserActiveDevicesAc(
    int PageNumber,
    int PageSize
    ) : IQuery<ApiPagedResponse<UserActiveDeviceAcDto>>, IPaginationQueryParams, IValidatable
{
    public void Validate(ICollection<ValidationMessage> validationMessages)
    {
        CustomValidators.PaginationParams.Validate(PageNumber, PageSize, validationMessages);
    }
}
