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
    private readonly HashSet<TestRatingId> _ratingIds;
    private readonly HashSet<TestCommentId> _commentIds;
    public AppUser(AppUserId id) {
        Id = id;
        _createdTestIds = [];
        _editorAssignedTests = [];
        _ratingIds = [];
        _commentIds = [];
    }

    public void AddCreatedTest(TestId testId) =>
        _createdTestIds.Add(testId);

    public void AddEditorRoleForTest(TestId testId) =>
        _editorAssignedTests.Add(testId);

    public void AddComment(TestCommentId commentId) => _commentIds.Add(commentId);
    public void AddRating(TestRatingId ratingId) => _ratingIds.Add(ratingId);
}