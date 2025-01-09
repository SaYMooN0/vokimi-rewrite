using AuthenticationService.Domain.Common;
using AuthenticationService.Domain.Common.value_objects;
using SharedKernel.Common;

namespace AuthenticationService.Domain.Events;

public record class UserConfirmedEvent(
    UnconfirmedAppUserId UnconfirmedAppUserId,
    Email Email,
    string PasswordHash
) : IDomainEvent;

