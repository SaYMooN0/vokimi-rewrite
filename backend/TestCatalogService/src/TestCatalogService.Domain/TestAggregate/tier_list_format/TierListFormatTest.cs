using System.Collections.Immutable;
using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.tests;
using TestCatalogService.Domain.TestAggregate.formats_shared;
using TestCatalogService.Domain.TestAggregate.formats_shared.events;
using TestCatalogService.Domain.TestTagAggregate.events;

namespace TestCatalogService.Domain.TestAggregate.tier_list_format;

public class TierListFormatTest : BaseTest
{
    private TierListFormatTest() { }
    public override TestFormat Format => TestFormat.TierList;
    public ushort TiersCount { get; }
    public ushort ItemsCount { get; }

    public TierListFormatTest(
        TestId testId, string name, string coverImg, string description, AppUserId creatorId,
        ImmutableArray<AppUserId> editorIds, DateTime publicationDate, Language language,
        ImmutableHashSet<TestTagId> tags, TestInteractionsAccessSettings interactionsAccessSettings,
        ushort tiersCount, ushort itemsCount
    ) : base(
        testId, name, coverImg, description, creatorId, editorIds,
        publicationDate, language, tags, interactionsAccessSettings
    ) {
        TiersCount = tiersCount;
        ItemsCount = itemsCount;
        _domainEvents.Add(new TestTagsChangedEvent(testId, new HashSet<TestTagId>(), tags));
        _domainEvents.Add(new NewPublishedTestCreatedEvent(testId, creatorId, editorIds.ToImmutableHashSet()));
    }
}