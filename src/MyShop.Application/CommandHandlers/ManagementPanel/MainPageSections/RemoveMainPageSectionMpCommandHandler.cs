using MyShop.Application.Commands.ManagementPanel.MainPageSections;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.MainPageSections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Application.CommandHandlers.ManagementPanel.MainPageSections;
internal sealed class RemoveMainPageSectionMpCommandHandler(
    IUnitOfWork unitOfWork
    ) : ICommandHandler<RemoveMainPageSectionMp>
{
    public async Task HandleAsync(RemoveMainPageSectionMp command, CancellationToken cancellationToken = default)
    {
        var entity = await unitOfWork.MainPageSectionRepository.GetByIdAsync(
            id: command.Id,
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException(nameof(MainPageSection), command.Id);

        await unitOfWork.RemoveAsync(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
