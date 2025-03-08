using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using TestCatalogService.Domain.Rules;
using TestCatalogService.Domain.TestAggregate;
using TestCatalogService.Domain.TestAggregate.formats_shared.events;
using TestCatalogService.Domain.TestCommentAggregate;
using TestCatalogService.Domain.UnitTests.FakeRepositories;

namespace TestCatalogService.Domain.UnitTests.TestAggregateRoot.formats_shared.comments_related_tests;

public class TestCommentReportingTests
{
    private readonly AppUserId _reportingUser = new(Guid.NewGuid());
    private readonly AppUserId _commentsAuthorId = new(Guid.NewGuid());
    private readonly string _defaultReportText = "Just some comment report text";
    private readonly CommentReportReason _reportReason = CommentReportReason.Custom;
    private static readonly List<TestComment> _repositoryCommentsLits = new();

    private readonly FakeTestCommentsRepository _fakeTestCommentsRepository = new() {
        AddFunc = (comment) => {
            _repositoryCommentsLits.Add(comment);
            return Task.CompletedTask;
        },
        GetByIdFunc = (id) => Task.FromResult(_repositoryCommentsLits.FirstOrDefault(c => c.Id == id)),
    };

    private readonly FakeAppUsersRepository _fakeAppUsersRepository = new() {
        DoesUserExistFunc = (id) => Task.FromResult(true)
    };


    private async Task<(BaseTest, List<TestCommentId>)> CreateTestWithComments(int commentsCount = 5) {
        var test = TestsSharedTestsConsts.CreateBaseTest();
        List<TestCommentId> testCommentIds = [];


        for (int i = 0; i < commentsCount; i++) {
            var comment = (await test.AddComment(
                _commentsAuthorId,
                "commentText daaah"
                , null,
                false,
                _fakeTestCommentsRepository,
                TestsSharedConsts.DateTimeProviderInstance
            )).GetSuccess();
            testCommentIds.Add(comment.Id);
        }

        return (test, testCommentIds);
    }

    [Fact]
    public async Task ReportComment_WhenCommentDoesNotExist_ShouldReturnNotFound() {
        // Arrange
        var (test, testCommentIds) = await CreateTestWithComments();
        var nonExistentCommentId = new TestCommentId(Guid.NewGuid());


        // Act
        var result = await test.ReportComment(
            _reportingUser,
            nonExistentCommentId,
            _defaultReportText,
            _reportReason,
            _fakeTestCommentsRepository,
            _fakeAppUsersRepository
        );

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Equal(Err.ErrCodes.NotFound, err.Code);
        Assert.Contains("Cannot report comment because this test does have this comment", err.Message);
    }


    [Fact]
    public async Task ReportComment_WhenEverythingIsOk_ShouldAddReportAndRaiseEvent() {
        // Arrange
        var (test, testCommentIds) = await CreateTestWithComments();
        var existingCommentId = testCommentIds[0];

        // Act
        var result = await test.ReportComment(
            _reportingUser,
            existingCommentId,
            _defaultReportText,
            _reportReason,
            _fakeTestCommentsRepository,
            _fakeAppUsersRepository
        );

        // Assert
        Assert.False(result.IsErr());
        var eventExists = test.GetDomainEventsCopy().Any(e =>
            e is TestCommentReportedEvent reportedEvent &&
            reportedEvent.CommentId == existingCommentId && reportedEvent.ReportAuthorId == _reportingUser
        );
        Assert.True(eventExists);
    }

    [Fact]
    public async Task ReportComment_WhenReportTextTooShort_ShouldReturnError() {
        // Arrange
        var (test, testCommentIds) = await CreateTestWithComments();
        var existingCommentId = testCommentIds[0];
        var tooShortText = new string('a', TestCommentReportRules.MinReportTextLength - 1);

        // Act
        var result = await test.ReportComment(
            _reportingUser,
            existingCommentId,
            tooShortText,
            _reportReason,
            _fakeTestCommentsRepository,
            _fakeAppUsersRepository
        );

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Equal(Err.ErrCodes.InvalidData, err.Code);
        Assert.Contains("The report text is too short", err.Message);
    }

    [Fact]
    public async Task ReportComment_WhenReportTextTooLong_ShouldReturnError() {
        // Arrange
        var (test, testCommentIds) = await CreateTestWithComments();
        var existingCommentId = testCommentIds[0];
        var tooLongText = new string('a', TestCommentReportRules.MaxReportTextLength + 1);

        // Act
        var result = await test.ReportComment(
            _reportingUser,
            existingCommentId,
            tooLongText,
            _reportReason,
            _fakeTestCommentsRepository,
            _fakeAppUsersRepository
        );

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Equal(Err.ErrCodes.InvalidData, err.Code);
        Assert.Contains("The report text is too long", err.Message);
    }
}