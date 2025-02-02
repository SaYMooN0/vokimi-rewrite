using SharedKernel.Common.domain;

namespace TestCatalogService.Domain.TestTagAggregate.events;
public record class TestTagsChangedEvent(HashSet<string> oldTags, HashSet<string> newTags) : IDomainEvent;
