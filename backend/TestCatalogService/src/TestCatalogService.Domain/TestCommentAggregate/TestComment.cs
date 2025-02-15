using System.Collections.Immutable;
using SharedKernel.Common.domain.aggregate_root;
using SharedKernel.Common.domain.entity_id;
using SharedKernel.Common.errors;
using TestCatalogService.Domain.Common;
using TestCatalogService.Domain.Rules;
using TestCatalogService.Domain.TestCommentAggregate.events;

namespace TestCatalogService.Domain.TestCommentAggregate;

public class TestComment : AggregateRoot<TestCommentId>
{
    private TestComment() { }
    public AppUserId Author { get; init; }
    public TestId TestId { get; init; }
    public TestComment? ParentComment { get; init; }
    private ICollection<TestComment> _childComments { get; init; }
    public string Text { get; init; }
    public TestCommentAttachment? Attachment { get; init; }
    public ImmutableArray<TestComment> ChildComments => _childComments.ToImmutableArray();
    //comment ratings

    public static ErrOr<TestComment> CreateNew(
        TestId testId, AppUserId author,
        string text, TestCommentAttachment? attachment
    ) {
        if (TestCommentRules.CheckCommentTextForErrs(text).IsErr(out var err)) {
            return err;
        }

        if (attachment is not null && attachment.CheckForErr().IsErr(out err)) {
            return err;
        }

        TestComment comment = new TestComment() {
            Id = TestCommentId.CreateNew(),
            TestId = testId,
            Author = author,
            ParentComment = null,
            _childComments = [],
            Text = text,
            Attachment = attachment
        };
        comment._domainEvents.Add(new NewTestCommentCreatedEvent(comment.Id, testId, author));
        return comment;
    }
}