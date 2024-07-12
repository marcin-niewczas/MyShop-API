using MyShop.Core.ValueObjects.Shared;

namespace MyShop.Core.Abstractions;
public interface IPasswordManager
{
    string SecurePassword(Password password);
    string SecureRefreshToken(string refreshToken);
    bool Verify(string password, string securedPassword);
}
