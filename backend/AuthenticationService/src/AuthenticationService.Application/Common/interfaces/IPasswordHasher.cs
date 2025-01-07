
namespace AuthenticationService.Application.Common.interfaces;

public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string hashedPassword, string passwordToCheck);
}
