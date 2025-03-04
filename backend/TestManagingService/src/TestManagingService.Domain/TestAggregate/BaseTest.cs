using System.Collections.Immutable;
using SharedKernel.Common.domain.aggregate_root;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests;
using TestManagingService.Domain.Common;
using TestManagingService.Domain.TestAggregate.formats_shared.comment_reports;

namespace TestManagingService.Domain.TestAggregate;

public abstract class BaseTest : AggregateRoot<TestId>
{
    protected BaseTest() { }
    public abstract TestFormat Format { get; }
    public AppUserId CreatorId { get; }
    public ImmutableArray<AppUserId> EditorIds { get; }
    public DateTime PublicationDate { get; }
    public ImmutableArray<TestFeedbackRecordId> _feedbackRecords { get; }

    protected ICollection<TestCommentReport> _commentReports { get; }

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
        _feedbackRecords = [];
        _commentReports = [];
    }

    public ErrOrNothing CheckUserAccessToManageTest(AppUserId userId) {
        if (userId != CreatorId) {
            return Err.ErrFactory.NoAccess("Only test creator has access to manage test");
        }

        return ErrOrNothing.Nothing;
    }
}