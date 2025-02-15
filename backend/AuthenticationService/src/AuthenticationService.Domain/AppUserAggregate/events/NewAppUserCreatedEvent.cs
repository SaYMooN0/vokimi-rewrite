using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity_id;

namespace AuthenticationService.Domain.AppUserAggregate.events;

public record NewAppUserCreatedEvent(AppUserId CreatedUserId) : IDomainEvent;
