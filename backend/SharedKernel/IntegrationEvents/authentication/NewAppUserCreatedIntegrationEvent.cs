using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;

namespace SharedKernel.IntegrationEvents.authentication;

public record class NewAppUserCreatedIntegrationEvent(AppUserId CreatedUserId) : IIntegrationEvent;