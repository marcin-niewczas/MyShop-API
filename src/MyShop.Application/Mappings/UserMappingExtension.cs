using MyShop.Application.Dtos.Account.Users;
using MyShop.Application.Dtos.Auth;
using MyShop.Core.Dtos.Auth;
using MyShop.Core.Models.Users;

namespace MyShop.Application.Mappings;
public static class UserMappingExtension
{
    public static AuthDto ToAuthDto(this UserToken entity, string accessToken, string refreshToken, DateTimeOffset expiryAccessTokenDate)
        => new()
        {
            AccessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken)),
            ExpiryAccessTokenDate = expiryAccessTokenDate != default
            ? expiryAccessTokenDate
            : throw new ArgumentException($"Param {nameof(expiryAccessTokenDate)} cannot be default.", nameof(expiryAccessTokenDate)),
            RefreshToken = refreshToken ?? throw new ArgumentNullException(nameof(refreshToken)),
            ExpiryRefreshTokenDate = entity.ExpiryRefreshTokenDate != default
                ? entity.ExpiryRefreshTokenDate
                : throw new ArgumentException(nameof(entity.ExpiryRefreshTokenDate)),
            UserTokenId = entity.Id,
            UserRole = entity.User.Role,
        };

    public static UserDto ToUserDto(this User entity)
        => entity switch
        {
            Customer customer => new CustomerMeDto()
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                Gender = customer.Gender,
                DateOfBirth = customer.DateOfBirth,
                UserRole = customer.Role,
                PhoneNumber = customer.PhoneNumber,
                PhotoUrl = customer.Photo?.Uri?.ToString(),
            },
            Employee employee => new EmployeeMeDto()
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                Gender = employee.Gender,
                DateOfBirth = employee.DateOfBirth,
                UserRole = employee.Role,
                PhoneNumber = employee.PhoneNumber,
                PhotoUrl = employee.Photo?.Uri?.ToString(),
                EmployeeRole = employee.EmployeeRole
            },
            Guest => new UserDto()
            {
                Id = entity.Id,
                UserRole = entity.Role,
            },
            _ => throw new NotImplementedException($"Not implemented conversion to dto for {entity.GetType().FullName} type.")
        };

    public static UserAddressAcDto ToUserAddressAcDto(this UserAddress entity)
        => new()
        {
            Id = entity.Id,
            StreetName = entity.StreetName,
            BuildingNumber = entity.BuildingNumber,
            ApartmentNumber = entity.ApartmentNumber,
            City = entity.City,
            ZipCode = entity.ZipCode,
            Country = entity.Country,
            UserAddressName = entity.UserAddressName,
            IsDefault = entity.IsDefault,
        };

    public static IReadOnlyCollection<UserAddressAcDto> ToUserAddressAcDtos(this IEnumerable<UserAddress> entities)
        => entities.Select(ToUserAddressAcDto).ToList();

    public static UserActiveDeviceAcDto ToUserActiveDeviceAcDto(this UserToken entity, Guid currentUserTokenId)
        => new()
        {
            BrowserFullName = entity.Browser.GetFullName(),
            OperatingSystem = entity.OperatingSystem,
            BrowserVersion = entity.BrowserVersion,
            IsMobile = entity.IsMobile,
            LastActivity = entity.UpdatedAt switch
            {
                DateTimeOffset updatedAt => updatedAt,
                null => entity.CreatedAt
            },
            IsCurrentDevice = currentUserTokenId == entity.Id
        };

    public static IReadOnlyCollection<UserActiveDeviceAcDto> ToUserActiveDeviceAcDtos(
        this IEnumerable<UserToken> entities,
        Guid currentUserTokenId
        ) => entities.Select(e => e.ToUserActiveDeviceAcDto(currentUserTokenId)).ToArray();
}
