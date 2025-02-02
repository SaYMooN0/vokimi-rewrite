using SharedKernel.Common.domain;

namespace AuthenticationService.Domain.AppUserAggregate.events;

public record NewAppUserCreatedEvent(AppUserId CreatedUserId) : IDomainEvent;
