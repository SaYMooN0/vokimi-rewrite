using SharedKernel.Common;
using SharedKernel.Common.EntityIds;

namespace AuthenticationService.Domain.AppUserAggregate.events;

public record NewAppUserCreatedEvent(AppUserId CreateUserId) : IDomainEvent;
