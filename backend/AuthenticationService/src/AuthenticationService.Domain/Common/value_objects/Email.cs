using SharedKernel.Common;
using SharedKernel.Common.errors;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AuthenticationService.Domain.Common.value_objects;

public class Email : ValueObject
{
    private string _email;
    private Email(string email) => _email = email;
    public static ErrOr<Email> Create(string email) {
        if (string.IsNullOrEmpty(email) || !IsStringValidEmail(email)) {
            return Err.ErrFactory.InvalidData(message: "Invalid email");
        }
        return new Email(email);
    }
    private const string emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

    public static bool IsStringValidEmail(string email) =>
        Regex.IsMatch(email, emailRegex);

    public override IEnumerable<object> GetEqualityComponents() {
        yield return _email;
    }
}
