using AuthenticationService.Domain.Common;
using AuthenticationService.Domain.Common.validation_rules;
using AuthenticationService.Domain.Common.value_objects;
using SharedKernel.Common;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using SharedKernel.Common.interfaces;

namespace AuthenticationService.Domain.UnconfirmedAppUserAggregate;

public class UnconfirmedAppUser : Entity
{
    protected override EntityId EntityId => Id;
    private UnconfirmedAppUser() { }

    public UnconfirmedAppUserId Id { get; init; }
    public string PasswordHash { get; private set; }
    public Email Email { get; init; }
    public DateTime CreationTime { get; init; }
    public string ConfirmationString { get; init; }

    public static ErrListOr<UnconfirmedAppUser> Create(
        string email,
        string password,
        Func<string, string> hashFunction,
        IDateTimeProvider dateTimeProvider
    ) {
        ErrList errList = new();

        var emailResult = Email.Create(email);
        errList.AddPossibleErr(emailResult);

        var passwordErr = PasswordRules.CheckForErr(password);
        errList.AddPossibleErr(passwordErr);

        if (errList.Any()) { return errList; }

        string passwordHash = hashFunction(password);
        string confirmationString = $"{DateTime.Now.GetHashCode()}-{Guid.NewGuid()}";

        return new UnconfirmedAppUser() {
            Id = UnconfirmedAppUserId.CreateNew(),
            PasswordHash = passwordHash,
            Email = emailResult.GetValue(),
            CreationTime = dateTimeProvider.Now,
            ConfirmationString = Guid.NewGuid().ToString()
        };
    }
}
