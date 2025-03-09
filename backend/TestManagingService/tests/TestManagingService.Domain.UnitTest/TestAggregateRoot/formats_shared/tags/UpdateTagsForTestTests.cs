using SharedKernel.Common;
using SharedKernel.Common.domain.entity;
using TestManagingService.Domain.TestAggregate.formats_shared.events;
using TestManagingService.Domain.UnitTest.TestAggregateRoot.test_consts;

namespace TestManagingService.Domain.UnitTest.TestAggregateRoot.formats_shared.tags;

public class UpdateTagsForTestTests
{
    [Fact]
    public void UpdateTags_WhenTooManyTagsProvided_ShouldReturnErr() {
        // Arrange
        var test = TestsSharedTestsConsts.CreateBaseTest();
        var excessiveTags = Enumerable.Range(0, TestTagsRules.MaxTagsForTestCount + 1)
            .Select(i => new TestTagId($"Tag{i}"))
            .ToHashSet();

        // Act
        var result = test.UpdateTags(excessiveTags);

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Equal($"Too many tags selected. Test cannot have more than {TestTagsRules.MaxTagsForTestCount}",
            err.Message);
        Assert.Contains(
            $"{excessiveTags.Count} tags selected. Maximum tags allowed: {TestTagsRules.MaxTagsForTestCount}",
            err.Details);
    }

    [Fact]
    public void UpdateTags_WhenTagsAreUnchanged_ShouldReturnErr() {
        // Arrange
        var test = TestsSharedTestsConsts.CreateBaseTest();
        var initialTags = new HashSet<TestTagId> { new("Tag1"), new("Tag2") };
        test.UpdateTags(initialTags);

        // Act
        var result = test.UpdateTags(initialTags);

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Equal("Tag list has not been changed", err.Message);
    }

    [Fact]
    public void UpdateTags_WhenTagsAreUpdated_ShouldReturnSuccessAndTriggerEvent() {
        // Arrange
        var test = TestsSharedTestsConsts.CreateBaseTest();
        var newTags = new HashSet<TestTagId> { new("Tag1"), new("Tag2") };

        // Act
        var result = test.UpdateTags(newTags);

        // Assert
        Assert.False(result.IsErr());
        Assert.Contains(test.GetDomainEventsCopy(),
            e => e is TagsChangedEvent tagsChanged
                 && tagsChanged.TestId == test.Id
                 && tagsChanged.NewTags.SetEquals(newTags)
        );
    }
}