using SharedKernel.Common.domain.entity;
using TestCatalogService.Domain.TestCommentAggregate.comment_attachments;

namespace TestCatalogService.Domain.UnitTests.TestCommentAggregateRoots.attachments;

public class TestCommentAttachmentGeneralTestResultTests
{
    [Fact]
    public void CheckForErr_WhenResultIdIsNull_ShouldReturnErr() {
        // Arrange
        CommentAttachmentGeneralTestResult attachment = new(null!);

        // Act
        var result = attachment.CheckForErr();

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Equal("Result Id is not set", err.Message);
    }

    [Fact]
    public void CheckForErr_WhenResultIdIsValid_ShouldReturnSucceed() {
        // Arrange
        var validResultId = GeneralTestResultId.CreateNew();
        CommentAttachmentGeneralTestResult attachment = new(validResultId);

        // Act
        var result = attachment.CheckForErr();

        // Assert
        Assert.False(result.IsErr());
    }
}