using System.Collections.Immutable;
using TestCatalogService.Domain.Rules;
using TestCatalogService.Domain.TestCommentAggregate.comment_attachments;

namespace TestCatalogService.Domain.UnitTests.TestCommentAggregateRoots.attachments;

public class TestCommentAttachmentImagesTests
{
    [Fact]
    public void CheckForErr_ShouldReturnErr_WhenImagesListIsEmpty() {
        // Arrange
        CommentAttachmentImages attachment = new([]);

        // Act
        var result = attachment.CheckForErr();

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Equal("Images list is empty", err.Message);
    }

    [Fact]
    public void CheckForErr_ShouldReturnErr_WhenImagePathIsEmpty() {
        // Arrange
        var images = ImmutableArray.Create("");
        var attachment = new CommentAttachmentImages(images);

        // Act
        var result = attachment.CheckForErr();

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Equal("Image path is empty", err.Message);
    }

    [Fact]
    public void CheckForErr_ShouldReturnErr_WhenImagePathIsTooLong() {
        // Arrange
        var longPath = new string('a', TestCommentRules.MaxAttachmentImagePathLen + 1);
        var images = ImmutableArray.Create(longPath);
        var attachment = new CommentAttachmentImages(images);

        // Act
        var result = attachment.CheckForErr();

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Equal("Image path is too long", err.Message);
        Assert.Equal("Try to somehow shorten it ", err.Details);
    }

    [Fact]
    public void CheckForErr_ShouldReturnNothing_WhenAllImagePathsAreValid() {
        // Arrange
        var validPaths = ImmutableArray.Create("valid/path1.png", "valid/path2.jpg");
        var attachment = new CommentAttachmentImages(validPaths);

        // Act
        var result = attachment.CheckForErr();

        // Assert
        Assert.False(result.IsErr());
    }
}