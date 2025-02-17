using SharedKernel.Common.domain;
using System.Collections.Immutable;
using SharedKernel.Common.domain.aggregate_root;
using SharedKernel.Common.domain.entity;
using TestCatalogService.Domain.Common;

namespace TestCatalogService.Domain.AppUserAggregate;

public class AppUser : AggregateRoot<AppUserId>
{
    private AppUser() { }
    private readonly HashSet<TestId> _createdTestIds;
    private readonly HashSet<TestId> _editorAssignedTests;
    private HashSet<TestCommentId> _commentIds { get; init; }
    private HashSet<TestRatingId> _ratingIds { get; init; }
    public ImmutableHashSet<TestId> CreatedTestIds => _createdTestIds.ToImmutableHashSet();
    public ImmutableHashSet<TestId> EditorAssignedTests => _editorAssignedTests.ToImmutableHashSet();
    public ImmutableHashSet<TestCommentId> CommentIds => _commentIds.ToImmutableHashSet();

    public ImmutableHashSet<TestRatingId> RatingIds => _ratingIds.ToImmutableHashSet();

    public AppUser(AppUserId id) {
        Id = id;
        _createdTestIds = [];
        _editorAssignedTests = [];
        _commentIds = [];
        _ratingIds = [];
    }

    public void AddCreatedTest(TestId testId) =>
        _createdTestIds.Add(testId);

    public void AddEditorRoleForTest(TestId testId) =>
        _editorAssignedTests.Add(testId);

    public void AddComment(TestCommentId commentId) => _commentIds.Add(commentId);
    public void AddRating(TestRatingId ratingId) => _ratingIds.Add(ratingId);
}