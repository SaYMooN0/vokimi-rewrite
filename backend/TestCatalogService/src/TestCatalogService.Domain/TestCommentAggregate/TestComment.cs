using System.Collections.Immutable;
using SharedKernel.Common.domain.aggregate_root;
using SharedKernel.Common.domain.entity_id;
using SharedKernel.Common.errors;
using SharedKernel.Common.interfaces;
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
    private ICollection<TestComment> _answers { get; init; }
    private ICollection<CommentVote> _votes { get; init; }
    public int CurrentVotesRating { get; private set; }
    public string Text { get; init; }
    public TestCommentAttachment? Attachment { get; init; }
    public DateTime CreatedAt { get; init; }
    public bool IsHidden { get; private set; }
    public ImmutableArray<TestComment> Answers => _answers.ToImmutableArray();

    public static ErrOr<TestComment> CreateNew(
        TestId testId, AppUserId author,
        string text, TestCommentAttachment? attachment,
        IDateTimeProvider dateTimeProvider
    ) {
        if (TestCommentRules.CheckCommentTextForErrs(text).IsErr(out var err)) {
            return err;
        }

        if (attachment is not null && attachment.CheckForErr().IsErr(out err)) {
            return err;
        }

        TestComment comment = new() {
            Id = TestCommentId.CreateNew(),
            TestId = testId,
            Author = author,
            ParentComment = null,
            _answers = [],
            _votes = [],
            CurrentVotesRating = 0,
            Text = text,
            Attachment = attachment,
            CreatedAt = dateTimeProvider.Now,
            IsHidden = false
        };
        comment._domainEvents.Add(new NewTestCommentCreatedEvent(comment.Id, testId, author));
        return comment;
    }

    public ErrOrNothing Vote(AppUserId user, bool isUp) {
        CommentVote? existingVote = _votes.FirstOrDefault(v => v.UserId == user);
        if (existingVote is null) {
            CommentVote vote = new(user, isUp);
            _votes.Add(vote);
            CurrentVotesRating += isUp ? 1 : -1;
            return ErrOrNothing.Nothing;
        }
        else if (existingVote.IsUp == isUp) {
            _votes.Remove(existingVote);
            CurrentVotesRating -= isUp ? 1 : -1;
            return ErrOrNothing.Nothing;
        }
        else if (existingVote.IsUp != isUp) {
            CurrentVotesRating += isUp ? 2 : -2;
            return existingVote.ChangeIsUpValue(isUp);
        }

        throw new Exception("Not every comment votes behaviour is defined");
    }

    public ErrOr<TestComment> AddAnswer(
        AppUserId author, string text,
        TestCommentAttachment? attachment,
        IDateTimeProvider dateTimeProvider
    ) {
        var answer = CreateNew(TestId, author, text, attachment, dateTimeProvider);
        if (answer.IsErr(out var err)) {
            return err;
        }

        _answers.Add(answer.GetSuccess());
        return answer.GetSuccess();
    }
}