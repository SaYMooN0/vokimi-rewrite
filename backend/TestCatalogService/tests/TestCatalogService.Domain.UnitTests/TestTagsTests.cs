using SharedKernel.Common;
using SharedKernel.Common.errors;
using TestCatalogService.Domain.Common;

namespace TestCatalogService.Domain.UnitTests;

public class TestTagsTests
{
    [Fact]
    public void Create_ValidTag_ReturnsTestTagId() {
        // Arrange
        string[] validTags = {
            "valid_tag_1",
            "tag123",
            "tag-123_valid",
            "validTag",
            "Test1234",
            "valid_tag_with_underscores",
            new string('a', TestTagsRules.MaxTagLength),  // Tag with exact length
        };

        // Act
        Dictionary<string, ErrOr<TestTagId>> creationResults = new();
        foreach (var tag in validTags) {
            creationResults.Add(tag, TestTagId.Create(tag));
        }

        // Assert
        foreach (var tag in validTags) {
            Assert.True(creationResults[tag].IsSuccess(out var tagId));
            Assert.Equal(tag, tagId.Value);
        }
    }

    [Fact]
    public void Create_InvalidTag_ReturnsError() {
        // Arrange
        string[] invalidTags = {
            "InValidTAg&&",
            "",
            "ThisTagIsWayTooLongForTheMaxTagLength",
            new string('a', TestTagsRules.MaxTagLength + 1),
            "tag with space",
            "!@#$%^&*",
        };

        Dictionary<string, ErrOr<TestTagId>> creationResults = new();

        // Act
        foreach (var tag in invalidTags) {
            creationResults.Add(tag, TestTagId.Create(tag));
        }

        // Assert
        foreach (var tag in invalidTags) {
            Assert.True(creationResults[tag].IsErr(out var err));
            Assert.Contains("is not a valid tag", err.Message);
        }
    }
}