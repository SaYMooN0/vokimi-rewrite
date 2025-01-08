using AuthenticationService.Application.Common.interfaces;

namespace AuthenticationService.Infrastructure.Services;

internal class PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password) {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string hashedPassword, string passwordToCheck) {
        return BCrypt.Net.BCrypt.Verify(passwordToCheck, hashedPassword);
    }
}
