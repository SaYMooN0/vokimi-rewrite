using SharedKernel.Common.EntityIds;

namespace SharedKernel.IntegrationEvents.authentication;

public record class NewAppUserCreatedIntegrationEvent(AppUserId CreatedUserId) : IIntegrationEvent;