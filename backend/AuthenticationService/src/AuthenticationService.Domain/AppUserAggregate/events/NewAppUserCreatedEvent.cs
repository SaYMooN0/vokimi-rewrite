using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;

namespace AuthenticationService.Domain.AppUserAggregate.events;

public record NewAppUserCreatedEvent(AppUserId CreatedUserId) : IDomainEvent;
