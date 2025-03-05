using MyShop.Application.Abstractions;
using MyShop.Application.Queries.ECommerce.Orders;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Orders;

namespace MyShop.Application.QueryHandlers.ECommerce.Orders;
internal sealed class GetInvoiceEcQueryHandler(
    IUnitOfWork unitOfWork,
    IInvoiceGenerator invoiceGenerator,
    IUserClaimsService userClaimsService
    ) : IQueryHandler<GetInvoiceEc, MemoryStream>
{
    public async Task<MemoryStream> HandleAsync(GetInvoiceEc query, CancellationToken cancellationToken = default)
    {
        var userId = userClaimsService.GetUserClaimsData().UserId;

        var result = await unitOfWork.InvoiceRepository.GetInvoiceWithOrderDetailsAsync(
            orderId: query.Id,
            invoiceId: query.InvoiceId,
            userId: userId,
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException(nameof(Invoice), query.InvoiceId);

        return invoiceGenerator.Generate(result);
    }
}
