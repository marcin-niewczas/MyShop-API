using MyShop.Application.Dtos.ManagementPanel.Dashboards;
using MyShop.Application.EndpointQueries.Interfaces;
using MyShop.Application.Responses;
using MyShop.Application.Validations.Interfaces;
using MyShop.Application.Validations.Validators;
using MyShop.Core.Validations;

namespace MyShop.Application.Queries.ManagementPanel.Dashboards;
public sealed record GetPagedDashboardDataMp(
    int PageNumber,
    int PageSize
    ) : IQuery<ApiPagedResponse<BaseDashboardElementMpDto>>,
        IPaginationQueryParams,
        IValidatable
{
    public void Validate(ICollection<ValidationMessage> validationMessages)
    {
        CustomValidators.PaginationParams.Validate(PageNumber, PageSize, validationMessages);
    }
}
