using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;
using TestCreationService.Domain.Rules;
using TestCreationService.Domain.TestAggregate.formats_shared.events;
using TestCreationService.Domain.TestAggregate.general_format;

namespace TestCreationService.Domain.UnitTests.TestAggregateRoot.general_format;

public class GeneralTestUpdateEditorsTests
{
    private readonly AppUserId _creatorId = new AppUserId(Guid.NewGuid());
    private readonly HashSet<AppUserId> _initialEditors = new()
    {
        new AppUserId(Guid.NewGuid()),
        new AppUserId(Guid.NewGuid())
    };

    private GeneralFormatTest CreateTest() {
        return GeneralFormatTest.CreateNew(_creatorId, "Test Name", _initialEditors)
            .GetSuccess();
    }

    [Fact]
    public void UpdateEditors_ShouldReturnError_WhenExceedingMaxEditors() {
        // Arrange
        var test = CreateTest();
        HashSet<AppUserId> tooManyEditors = [];
        for (int i = 0; i < TestRules.MaxTestEditorsCount + 1; i++) {
            tooManyEditors.Add(new AppUserId(Guid.NewGuid()));
        }

        // Act
        var result = test.UpdateEditors(tooManyEditors);

        // Assert
        Assert.True(result.IsErr(), "Expected an error due to exceeding the max editors limit.");
        Assert.True(result.IsErr(out var err));
        Assert.Equal("Too many test editors", err.Message);
    }

    [Fact]
    public void UpdateEditors_ShouldReturnError_WhenCreatorIsInEditors() {
        // Arrange
        var test = CreateTest();
        var newEditors = new HashSet<AppUserId>(_initialEditors) { _creatorId };

        // Act
        var result = test.UpdateEditors(newEditors);

        // Assert
        Assert.True(result.IsErr(), "Expected an error when the creator is included in the editors.");
        Assert.True(result.IsErr(out var err));
        Assert.Equal("Test creator can't be editor", err.Message);
    }

    [Fact]
    public void UpdateEditors_ShouldUpdateSuccessfully_WhenValidEditorsProvided() {
        // Arrange
        var test = CreateTest();
        var newEditors = new HashSet<AppUserId>
        {
            new AppUserId(Guid.NewGuid()),
            new AppUserId(Guid.NewGuid())
        };

        // Act
        var result = test.UpdateEditors(newEditors);

        // Assert
        Assert.False(result.IsErr(), "Expected no error when valid editors are provided.");
        Assert.Equal(newEditors, test.EditorIds);
    }

    [Fact]
    public void UpdateEditors_ShouldTriggerDomainEvent() {
        // Arrange
        var test = CreateTest();
        var newEditors = new HashSet<AppUserId>
        {
            new AppUserId(Guid.NewGuid()),
            new AppUserId(Guid.NewGuid())
        };

        // Act
        var result = test.UpdateEditors(newEditors);

        // Assert
        Assert.False(result.IsErr(), "Expected no error when valid editors are provided.");
        Assert.Contains(test.GetDomainEventsCopy(), e =>
            e is TestEditorsListChangedEvent ev && ev.NewEditors.SetEquals(newEditors) && ev.PreviousEditors.SetEquals(_initialEditors));
    }
}
