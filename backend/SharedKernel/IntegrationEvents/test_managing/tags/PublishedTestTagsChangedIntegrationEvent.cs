using SharedKernel.Common.domain.entity;

namespace SharedKernel.IntegrationEvents.test_managing.tags;

public record PublishedTestTagsChangedIntegrationEvent(
    TestId TestId,
    string[] NewTags
) : IIntegrationEvent;