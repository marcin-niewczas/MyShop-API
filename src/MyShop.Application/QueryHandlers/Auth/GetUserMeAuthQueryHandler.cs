using MyShop.Application.Abstractions;
using MyShop.Application.Mappings;
using MyShop.Application.Queries.Auth;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Dtos.Auth;
using MyShop.Core.Exceptions;
using MyShop.Core.HelperModels;
using MyShop.Core.Models.Users;

namespace MyShop.Application.QueryHandlers.Auth;
internal sealed class GetUserMeAuthQueryHandler(
    IUnitOfWork unitOfWork,
    IUserClaimsService userClaimsService
    ) : IQueryHandler<GetUserMeAuth, ApiResponse<UserDto>>
{
    public async Task<ApiResponse<UserDto>> HandleAsync(GetUserMeAuth query, CancellationToken cancellationToken = default)
    {
        var userClaimsData = userClaimsService.GetUserClaimsData();

        var user = userClaimsData switch
        {
            CustomerClaimsData => await unitOfWork.RegisteredUserRepository.GetByIdAsync(
                userClaimsData.UserId,
                includeExpression: i => i.Photo,
                cancellationToken: cancellationToken
            ) ?? throw new NotFoundException($"Not found {nameof(User)}."),
            UserClaimsData => await unitOfWork.UserRepository.GetByIdAsync(
                userClaimsData.UserId,
                cancellationToken: cancellationToken
            ) ?? throw new NotFoundException($"Not found {nameof(User)}."),
        };

        return new(user.ToUserDto());
    }
}
