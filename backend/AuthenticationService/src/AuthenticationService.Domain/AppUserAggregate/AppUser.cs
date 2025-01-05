using AuthenticationService.Domain.Common.value_objects;
using AuthenticationService.Domain.UnconfirmedAppUserAggregate;
using SharedKernel.Common;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.interfaces;

namespace AuthenticationService.Domain.AppUserAggregate;

public class AppUser : AggregateRoot
{
    protected override EntityId EntityId => Id;
    private AppUser() { }

    public AppUserId Id { get; init; }
    internal string PasswordHash { get; private set; }
    internal Email Email { get; init; }
    public DateOnly RegistrationDate { get; init; }

    public static AppUser FromUnconfirmed(
        UnconfirmedAppUser unconfirmedUser,
        IDateTimeProvider dateTimeProvider
    ) => new() {
        Id = AppUserId.CreateNew(),
        PasswordHash = unconfirmedUser.PasswordHash,
        Email = unconfirmedUser.Email,
        RegistrationDate = dateTimeProvider.NowDateOnly
    };
}
