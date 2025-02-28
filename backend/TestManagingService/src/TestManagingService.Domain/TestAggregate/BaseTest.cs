using System.Collections.Immutable;
using SharedKernel.Common.domain.aggregate_root;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests;

namespace TestManagingService.Domain.TestAggregate;

public abstract class BaseTest : AggregateRoot<TestId>
{
    protected BaseTest() { }
    public abstract TestFormat Format { get; }
    public AppUserId CreatorId { get; init; }
    public ImmutableArray<AppUserId> EditorIds { get; init; }
    public DateTime PublicationDate { get; init; }

    protected BaseTest(
        TestId testId,
        AppUserId creatorId,
        ImmutableArray<AppUserId> editorIds,
        DateTime publicationDate
    ) {
        Id = testId;
        CreatorId = creatorId;
        EditorIds = editorIds;
        PublicationDate = publicationDate;
    }

    public ErrOrNothing CheckUserAccessToManageTest(AppUserId userId) {
        if (userId != CreatorId) {
            return Err.ErrFactory.NoAccess("Only test creator has access to manage test");
        }

        return ErrOrNothing.Nothing;
    }

}