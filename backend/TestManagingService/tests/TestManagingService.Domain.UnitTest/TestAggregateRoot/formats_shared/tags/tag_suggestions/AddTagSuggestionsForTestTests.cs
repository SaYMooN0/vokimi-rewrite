using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.tests.value_objects;
using TestManagingService.Domain.TestAggregate;
using TestManagingService.Domain.TestAggregate.formats_shared;
using TestManagingService.Domain.UnitTest.TestAggregateRoot.test_consts;

namespace TestManagingService.Domain.UnitTest.TestAggregateRoot.formats_shared.tags.tag_suggestions;

public class AddTagSuggestionsForTestTests
{
    [Fact]
    public void AddTagSuggestions_WhenTagSuggestionsAreNotAllowed_ShouldReturnError() {
        // Arrange
        var test = TestsSharedTestsConsts.CreateTest(
            new TestInteractionsAccessSettings(
                AccessLevel.Public,
                ResourceAvailabilitySetting.EnabledPublic,
                ResourceAvailabilitySetting.EnabledPublic,
                allowTagsSuggestions: false,
                allowTestTakenPosts: true
            )
        );

        // Act
        var result = test.AddTagSuggestions([new TestTagId("new_test_tag_id")],
            TestsSharedTestsConsts.DateTimeProviderInstance);

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Equal("Tag suggestions are not allowed for this test", err.Message);
    }

    [Fact]
    public void AddTagSuggestions_WhenTooManyTagsAreSuggested_ShouldReturnError() {
        // Arrange
        var test = TestsSharedTestsConsts.CreateTest();
        var tooManyTags = Enumerable.Range(0, BaseTest.MaxSuggestionsCountToInteract + 1)
            .Select(_ => new TestTagId(Guid.NewGuid().ToString()))
            .ToHashSet();

        // Act
        var result = test.AddTagSuggestions(tooManyTags, TestsSharedTestsConsts.DateTimeProviderInstance);

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Contains($"Cannot add more than {BaseTest.MaxSuggestionsCountToInteract} tag suggestions", err.Message);
    }

    [Fact]
    public void AddTagSuggestions_WhenAllTagsAreAlreadyAdded_ShouldReturnError() {
        // Arrange
        var existingTags = new[] { new TestTagId("existing_tag_1"), new TestTagId("existing_tag_2") };
        var test = TestsSharedTestsConsts.CreateTest(testTags: existingTags);

        // Act
        var result = test.AddTagSuggestions(existingTags.ToHashSet(), TestsSharedTestsConsts.DateTimeProviderInstance);

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Equal(
            "Cannot accept tag suggestions because every tag is either already in the test or banned from suggestion.",
            err.Message);
    }

    [Fact]
    public void AddTagSuggestions_WhenAllTagsAreAlreadyBanned_ShouldReturnError() {
        // Arrange
        var bannedTags = new[] { new TestTagId("banned_tag_1"), new TestTagId("banned_tag_2") };
        var test = TestsSharedTestsConsts.CreateTest(bannedTags: bannedTags);

        // Act
        var result = test.AddTagSuggestions(bannedTags.ToHashSet(), TestsSharedTestsConsts.DateTimeProviderInstance);

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Equal(
            "Cannot accept tag suggestions because every tag is either already in the test or banned from suggestion.",
            err.Message);
    }

    [Fact]
    public void AddTagSuggestions_WhenAllTagsAreAlreadyAddedOrBanned_ShouldReturnError() {
        // Arrange
        var addedTags = new[] { new TestTagId("added_tag_1"), new TestTagId("added_tag_2") };
        var bannedTags = new[] { new TestTagId("banned_tag_1"), new TestTagId("banned_tag_2") };
        var allTags = addedTags.Concat(bannedTags).ToArray();
        var test = TestsSharedTestsConsts.CreateTest(testTags: addedTags, bannedTags: bannedTags);

        // Act
        var result = test.AddTagSuggestions(allTags.ToHashSet(), TestsSharedTestsConsts.DateTimeProviderInstance);

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Equal(
            "Cannot accept tag suggestions because every tag is either already in the test or banned from suggestion.",
            err.Message);
    }

    [Fact]
    public void AddTagSuggestions_WhenEverythingIsCorrect_ShouldAddNewAndIncrementExisting() {
        // Arrange
        var existingTag = new TestTagId("existing_test_tag");
        var newTag = new TestTagId("new_test_tag");
        var test = TestsSharedTestsConsts.CreateTest(tagSuggestions: [existingTag]);

        // Act
        var result = test.AddTagSuggestions([existingTag, newTag], TestsSharedTestsConsts.DateTimeProviderInstance);

        // Assert
        Assert.False(result.IsErr());
        Assert.Contains(test.TagSuggestions, s => s.Tag == newTag);
        Assert.Contains(test.TagSuggestions, s => s.Tag == existingTag && s.Count > 1);
    }

    [Fact]
    public void AddTagSuggestions_WhenSomeSuggestedTagsAreAdded_ShouldAddNewAndIncrementExisting() {
        // Arrange
        var existingTag = new TestTagId("existing_test_tag");
        var newTag = new TestTagId("new_test_tag");
        var test = TestsSharedTestsConsts.CreateTest(tagSuggestions: [existingTag]);

        // Act
        var result = test.AddTagSuggestions([existingTag, newTag], TestsSharedTestsConsts.DateTimeProviderInstance);

        // Assert
        Assert.False(result.IsErr());
        Assert.Contains(test.TagSuggestions, s => s.Tag == newTag);
        Assert.Contains(test.TagSuggestions, s => s.Tag == existingTag && s.Count > 1);
    }

    [Fact]
    public void AddTagSuggestions_WhenSomeSuggestedTagsAreBanned_ShouldAddNewAndIncrementExisting() {
        // Arrange
        var bannedTag = new TestTagId("banned_test_tag");
        var existingTag = new TestTagId("existing_test_tag");
        var newTag = new TestTagId("new_test_tag");
        var test = TestsSharedTestsConsts.CreateTest(tagSuggestions: [existingTag], bannedTags: [bannedTag]);

        // Act
        var result = test.AddTagSuggestions([bannedTag, existingTag, newTag],
            TestsSharedTestsConsts.DateTimeProviderInstance);

        // Assert
        Assert.False(result.IsErr());
        Assert.DoesNotContain(test.TagSuggestions, s => s.Tag == bannedTag);
        Assert.Contains(test.TagSuggestions, s => s.Tag == newTag);
        Assert.Contains(test.TagSuggestions, s => s.Tag == existingTag && s.Count > 1);
    }

    [Fact]
    public void AddTagSuggestions_WhenSomeSuggestedTagsAreAddedOrBanned_ShouldAddNewAndIncrementExisting() {
        // Arrange
        var bannedTag = new TestTagId("banned_test_tag");
        var existingTag = new TestTagId("existing_test_tag");
        var newTag = new TestTagId("new_test_tag");
        var anotherNewTag = new TestTagId("another_new_test_tag");
        var test = TestsSharedTestsConsts.CreateTest(tagSuggestions: [existingTag], bannedTags: [bannedTag]);

        // Act
        var result = test.AddTagSuggestions([bannedTag, existingTag, newTag, anotherNewTag],
            TestsSharedTestsConsts.DateTimeProviderInstance);

        // Assert
        Assert.False(result.IsErr());
        Assert.DoesNotContain(test.TagSuggestions, s => s.Tag == bannedTag);
        Assert.Contains(test.TagSuggestions, s => s.Tag == newTag);
        Assert.Contains(test.TagSuggestions, s => s.Tag == anotherNewTag);
        Assert.Contains(test.TagSuggestions, s => s.Tag == existingTag && s.Count > 1);
    }
}