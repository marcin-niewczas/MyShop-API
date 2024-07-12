using Microsoft.AspNetCore.Identity;
using MyShop.Core.Abstractions;
using MyShop.Core.Models.Users;
using MyShop.Core.ValueObjects.Shared;

namespace MyShop.Infrastructure.Security;
internal sealed class PasswordManager(
    IPasswordHasher<User> passwordHasher
    ) : IPasswordManager
{
    public string SecurePassword(Password password)
        => passwordHasher.HashPassword(default!, password);

    public string SecureRefreshToken(string refreshToken)
        => passwordHasher.HashPassword(default!, refreshToken);

    public bool Verify(string password, string securedPassword)
        => passwordHasher.VerifyHashedPassword(
            default!,
            securedPassword,
            password
            ) is PasswordVerificationResult.Success;
}
