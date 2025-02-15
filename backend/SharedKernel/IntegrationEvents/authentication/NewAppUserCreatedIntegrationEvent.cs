using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity_id;

namespace SharedKernel.IntegrationEvents.authentication;

public record class NewAppUserCreatedIntegrationEvent(AppUserId CreatedUserId) : IIntegrationEvent;