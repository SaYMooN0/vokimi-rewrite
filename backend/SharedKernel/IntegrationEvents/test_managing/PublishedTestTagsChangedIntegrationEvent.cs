using SharedKernel.Common.domain.entity;

namespace SharedKernel.IntegrationEvents.test_managing;

public record PublishedTestTagsChangedIntegrationEvent(
    TestId TestId,
    string[] NewTags
) : IIntegrationEvent;