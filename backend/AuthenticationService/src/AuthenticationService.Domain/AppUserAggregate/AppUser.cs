using AuthenticationService.Domain.AppUserAggregate.events;
using AuthenticationService.Domain.Common.value_objects;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.aggregate_root;
using SharedKernel.Common.domain.entity_id;
using SharedKernel.Common.interfaces;

namespace AuthenticationService.Domain.AppUserAggregate;

public class AppUser : AggregateRoot<AppUserId>
{
    private AppUser() { }
    public Email Email { get; init; }
    public string PasswordHash { get; private set; }
    public DateOnly RegistrationDate { get; init; }
    public AppUserRole Role { get; private set; }

    public static AppUser CreateNew(
        Email email,
        string passwordHash,
        IDateTimeProvider dateTimeProvider
    ) {
        AppUser user = new() {
            Id = AppUserId.CreateNew(),
            Email = email,
            PasswordHash = passwordHash,
            RegistrationDate = dateTimeProvider.NowDateOnly,
            Role = AppUserRole.Member
        };
        user._domainEvents.Add(new NewAppUserCreatedEvent(user.Id));
        return user;
    }
    public bool IsPasswordCorrect(Func<string, bool> passwordHashFunc) {
        return passwordHashFunc(PasswordHash);
    }
}
