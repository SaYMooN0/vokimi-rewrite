using SharedKernel.Common.domain.entity;
using TestManagingService.Domain.TestAggregate;
using TestManagingService.Domain.TestAggregate.formats_shared.events;
using TestManagingService.Domain.UnitTest.TestAggregateRoot.test_consts;

namespace TestManagingService.Domain.UnitTest.TestAggregateRoot.formats_shared.tags.tag_suggestions;

public class AcceptTagSuggestionsForTestTests
{
    [Fact]
    public void AcceptTagSuggestions_WhenTooManyTagsSpecified_ShouldReturnErr() {
        // Arrange
        var initialSuggestions = Enumerable.Range(0, BaseTest.MaxSuggestionsCountToInteract)
            .Select(i => new TestTagId($"suggested_tag_{i}"))
            .ToArray();

        var test = TestsSharedTestsConsts.CreateTest(tagSuggestions: initialSuggestions);
        var additionalSuggestion1 = new TestTagId("tag1");
        var additionalSuggestion2 = new TestTagId("tag2");
        test.AddTagSuggestions(
            [additionalSuggestion1, additionalSuggestion2],
            TestsSharedTestsConsts.DateTimeProviderInstance
        );

        HashSet<TestTagId> suggestionsToAccept = [..initialSuggestions, additionalSuggestion1, additionalSuggestion2];
        // Act
        var result = test.AcceptTagSuggestions(suggestionsToAccept);

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Contains($"Cannot accept more than {BaseTest.MaxSuggestionsCountToInteract}", err.Message);
    }

    [Fact]
    public void AcceptTagSuggestions_WhenSomeTagsWereNotSuggested_ShouldReturnErr() {
        // Arrange
        var suggestedTag = new TestTagId("suggested_tag");
        var notSuggestedTag = new TestTagId("not_suggested_tag");
        var test = TestsSharedTestsConsts.CreateTest(tagSuggestions: [suggestedTag]);

        // Act
        var result = test.AcceptTagSuggestions([suggestedTag, notSuggestedTag]);

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Equal(
            "Unable to accept tag suggestions because one or more tags were not suggested. Please remove these tags now and add them manually.",
            err.Message);
        Assert.Contains("not_suggested_tag", err.Details);
    }

    

    [Fact]
    public void AcceptTagSuggestions_WhenNoTagsArePassed_ShouldReturnErr() {
        // Arrange
        var test = TestsSharedTestsConsts.CreateTest();

        // Act
        var result = test.AcceptTagSuggestions([]);

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Equal("No tags specified for acceptance", err.Message);
    }

    [Fact]
    public void AcceptTagSuggestions_WhenAcceptingSubsetOfSuggestedTags_ShouldAddNewTagsAndRemoveAcceptedSuggestions() {
        // Arrange
        var suggested1 = new TestTagId("suggested_tag_1");
        var suggested2 = new TestTagId("suggested_tag_2");
        var test = TestsSharedTestsConsts.CreateTest(tagSuggestions: [suggested1, suggested2]);

        // Act
        var result = test.AcceptTagSuggestions([suggested1]);

        // Assert
        Assert.False(result.IsErr());
        Assert.Contains(suggested1, test.Tags);
        Assert.DoesNotContain(test.TagSuggestions, s => s.Tag == suggested1);
        Assert.Contains(test.TagSuggestions, s => s.Tag == suggested2);

        Assert.Contains(test.GetDomainEventsCopy(), e =>
            e is TagsChangedEvent evt &&
            evt.TestId == test.Id &&
            evt.NewTags.Contains(suggested1)
        );
    }

    [Fact]
    public void AcceptTagSuggestions_WhenAcceptingAllOfSuggestedTags_ShouldAddNewTagsAndRemoveAcceptedSuggestions() {
        // Arrange
        var suggested1 = new TestTagId("suggested_tag_1");
        var suggested2 = new TestTagId("suggested_tag_2");
        var test = TestsSharedTestsConsts.CreateTest(tagSuggestions: [suggested1, suggested2]);

        // Act
        var result = test.AcceptTagSuggestions([suggested1, suggested2]);

        // Assert
        Assert.False(result.IsErr());
        Assert.Contains(suggested1, test.Tags);
        Assert.Contains(suggested2, test.Tags);
        Assert.Empty(test.TagSuggestions);

        Assert.Contains(test.GetDomainEventsCopy(), e =>
            e is TagsChangedEvent evt &&
            evt.TestId == test.Id &&
            evt.NewTags.Contains(suggested1) &&
            evt.NewTags.Contains(suggested2)
        );
    }
}