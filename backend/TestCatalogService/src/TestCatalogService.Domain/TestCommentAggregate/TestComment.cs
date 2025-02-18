using System.Collections.Immutable;
using SharedKernel.Common.domain.aggregate_root;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.interfaces;
using TestCatalogService.Domain.Common;
using TestCatalogService.Domain.Rules;

namespace TestCatalogService.Domain.TestCommentAggregate;

public class TestComment : AggregateRoot<TestCommentId>, ISoftDeleteableEntity
{
    private TestComment() { }
    public AppUserId AuthorId { get; init; }
    public TestId TestId { get; init; }
    public TestComment? ParentComment { get; init; }
    private ICollection<TestComment> _answers { get; init; }
    public uint CurrentAnswersCount { get; private set; }
    private ICollection<CommentVote> _votes { get; init; }
    public uint UpVotesCount { get; private set; }
    public uint DownVotesCount { get; private set; }
    private string _text { get; init; }
    private TestCommentAttachment? _attachment { get; init; }
    public DateTime CreatedAt { get; init; }
    public bool IsHidden { get; private set; }
    public bool MarkedAsSpoiler { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    public ErrOr<string> Text =>
        IsDeleted ? Err.ErrFactory.NoAccess("Cannot access deleted comment text") : _text;

    public ErrOr<TestCommentAttachment?> Attachment =>
        IsDeleted ? Err.ErrFactory.NoAccess("Cannot access deleted comment attachment") : _attachment;

    public IReadOnlyCollection<TestComment> Answers => _answers.ToImmutableArray();
    public IReadOnlyCollection<CommentVote> Votes => _votes.ToImmutableArray();
    public bool HasAttachment => _attachment is not null;

    public static ErrOr<TestComment> CreateNew(
        TestId testId,
        AppUserId authorId,
        string text,
        TestCommentAttachment? attachment,
        bool markAsSpoiler,
        IDateTimeProvider dateTimeProvider
    ) {
        if (TestCommentRules.CheckCommentTextForErrs(text).IsErr(out var err)) {
            return err;
        }

        if (attachment is not null && attachment.CheckForErr().IsErr(out err)) {
            return err;
        }

        return new TestComment() {
            Id = TestCommentId.CreateNew(),
            TestId = testId,
            AuthorId = authorId,
            _answers = [],
            CurrentAnswersCount = 0,
            _votes = [],
            UpVotesCount = 0,
            DownVotesCount = 0,
            _text = text,
            _attachment = attachment,
            CreatedAt = dateTimeProvider.Now,
            IsHidden = false,
            MarkedAsSpoiler = markAsSpoiler
        };
    }

    public ErrOrNothing Vote(AppUserId user, bool isUp) {
        CommentVote? existingVote = _votes.FirstOrDefault(v => v.UserId == user);
        if (existingVote is null) {
            CommentVote vote = new(user, isUp);
            _votes.Add(vote);
            if (isUp) {
                UpVotesCount++;
            }
            else {
                DownVotesCount++;
            }

            return ErrOrNothing.Nothing;
        }
        else if (existingVote.IsUp == isUp) {
            _votes.Remove(existingVote);
            if (existingVote.IsUp) {
                UpVotesCount--;
            }
            else {
                DownVotesCount--;
            }

            return ErrOrNothing.Nothing;
        }
        else if (existingVote.IsUp != isUp) {
            if (isUp) {
                UpVotesCount++;
                DownVotesCount--;
            }
            else {
                UpVotesCount--;
                DownVotesCount++;
            }

            return existingVote.ChangeIsUpValue(isUp);
        }

        throw new Exception("Not every comment votes behaviour is defined");
    }

    public void AddAnswer(TestComment answer) {
        _answers.Add(answer);
        CurrentAnswersCount += 1;
    }

    public ErrOrNothing Delete(IDateTimeProvider timeProvider) {
        if (IsDeleted) {
            return new Err("This comment is already deleted");
        }

        IsDeleted = true;
        DeletedAt = timeProvider.Now;

        return ErrOrNothing.Nothing;
    }
}