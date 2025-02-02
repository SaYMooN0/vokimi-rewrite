using SharedKernel.Common.domain;

namespace SharedKernel.IntegrationEvents.authentication;

public record class NewAppUserCreatedIntegrationEvent(AppUserId CreatedUserId) : IIntegrationEvent;