using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.interfaces;
using TestCatalogService.Domain.TestAggregate;
using TestCatalogService.Domain.TestCommentAggregate;
using TestCatalogService.Domain.UnitTests.FakeRepositories;
using TestCatalogService.Domain.UnitTests.TestAggregateRoot.test_consts;

namespace TestCatalogService.Domain.UnitTests.TestAggregateRoot.formats_shared.comments_related_tests;

public class TestAnsweringToCommentTests
{
    private readonly AppUserId _authorId = new(Guid.NewGuid());
    private readonly TestCommentAttachment? _attachment = null;
    private readonly bool _markedAsSpoiler = false;
    private readonly string _defaultCommentText = "This is an answer to the comment";

    private static readonly List<TestComment> _repositoryCommentsList = new();

    private readonly FakeTestCommentsRepository _fakeTestCommentsRepository = new() {
        GetByIdFunc = (id) => Task.FromResult(_repositoryCommentsList.FirstOrDefault(c => c.Id == id)),
        UpdateFunc = (comment) => {
            var existingComment = _repositoryCommentsList.FirstOrDefault(c => c.Id == comment.Id);
            if (existingComment != null) {
                _repositoryCommentsList.Remove(existingComment);
            }

            _repositoryCommentsList.Add(comment);
            return Task.CompletedTask;
        },
        AddFunc = (comment) => {
            _repositoryCommentsList.Add(comment);
            return Task.CompletedTask;
        }
    };

    private readonly IDateTimeProvider _dateTimeProvider = TestsSharedConsts.DateTimeProviderInstance;

    private async Task<(BaseTest test, TestComment parentComment)> CreateTestWithComment() {
        var test = TestsSharedTestsConsts.CreateBaseTest();
        var parentComment = (await test.AddComment(
            _authorId, "Parent comment", null, false, _fakeTestCommentsRepository, _dateTimeProvider
        )).GetSuccess();

        _repositoryCommentsList.Add(parentComment);

        return (test, parentComment);
    }

    [Fact]
    public async Task AddAnswerToComment_WhenSuccessful_ShouldReturnCreatedComment() {
        // Arrange
        var (test, parentComment) = await CreateTestWithComment();

        // Act
        var result = await test.AddAnswerToComment(
            parentComment.Id, _authorId, _defaultCommentText, _attachment, _markedAsSpoiler,
            _fakeTestCommentsRepository, _dateTimeProvider
        );

        // Assert
        Assert.True(result.IsSuccess(out var answerComment));
        Assert.NotNull(answerComment);
        Assert.Equal(_defaultCommentText, answerComment.Text.GetSuccess());
        Assert.Contains(answerComment, parentComment.Answers);
    }

    [Fact]
    public async Task AddAnswerToComment_WhenParentCommentDoesNotExist_ShouldReturnNotFound() {
        // Arrange
        var test = TestsSharedTestsConsts.CreateBaseTest();
        var nonExistentCommentId = new TestCommentId(Guid.NewGuid());

        // Act
        var result = await test.AddAnswerToComment(
            nonExistentCommentId, _authorId, _defaultCommentText, _attachment, _markedAsSpoiler,
            _fakeTestCommentsRepository, _dateTimeProvider
        );

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Equal(Err.ErrCodes.NotFound, err.Code);
        Assert.Contains("Cannot find parent comment", err.Message);
    }

    [Fact]
    public async Task AddAnswerToComment_WhenParentCommentBelongsToAnotherTest_ShouldReturnError() {
        // Arrange
        var (test, parentComment) = await CreateTestWithComment();
        var anotherTest = TestsSharedTestsConsts.CreateBaseTest();

        // Act
        var result = await anotherTest.AddAnswerToComment(
            parentComment.Id, _authorId, _defaultCommentText, _attachment, _markedAsSpoiler,
            _fakeTestCommentsRepository, _dateTimeProvider
        );

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Contains("Parent comment doesn't belong to this test", err.Message);
    }
}