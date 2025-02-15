using AuthenticationService.Domain.AppUserAggregate.events;
using AuthenticationService.Domain.Common;
using AuthenticationService.Domain.Common.value_objects;
using AuthenticationService.Domain.Rules;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.aggregate_root;
using SharedKernel.Common.errors;
using SharedKernel.Common.interfaces;

namespace AuthenticationService.Domain.UnconfirmedAppUserAggregate;

public class UnconfirmedAppUser : AggregateRoot<UnconfirmedAppUserId>
{
    private UnconfirmedAppUser() { }
    private string PasswordHash { get;  set; }
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

        return new UnconfirmedAppUser() {
            Id = UnconfirmedAppUserId.CreateNew(),
            PasswordHash = passwordHash,
            Email = emailResult.GetSuccess(),
            CreationTime = dateTimeProvider.Now,
            ConfirmationString = Guid.NewGuid().ToString()
        };
    }
    public ErrOrNothing Confirm(string confirmationString) {
        if (confirmationString != this.ConfirmationString) {
            return new Err(message: "Unable to confirm user. Incorrect confirmation string was provided");
        }
        _domainEvents.Add(new UserConfirmedEvent(Id, Email, PasswordHash));
        return ErrOrNothing.Nothing;
    }
}
