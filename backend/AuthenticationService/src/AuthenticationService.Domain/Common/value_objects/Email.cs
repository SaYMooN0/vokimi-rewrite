using SharedKernel.Common;
using SharedKernel.Common.errors;
using System.Text.RegularExpressions;

namespace AuthenticationService.Domain.Common.value_objects;

internal class Email : ValueObject
{
    private string _email;
    private Email(string email) => _email = email;
    public static ErrOr<Email> Create(string email) {
        if (string.IsNullOrEmpty(email) || !IsValidEmail(email)) {
            return Err.ErrFactory.InvalidData(message: "Invalid email");
        }
        return new Email(email);
    }

    public override IEnumerable<object> GetEqualityComponents() {
        yield return _email;
    }
    private static bool IsValidEmail(string email) {
        var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, emailPattern);
    }
}
