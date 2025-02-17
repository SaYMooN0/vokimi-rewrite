using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using TestCreationService.Domain.Rules;
using TestCreationService.Domain.TestAggregate.formats_shared.events;
using TestCreationService.Domain.TestAggregate.general_format;

namespace TestCreationService.Domain.UnitTests.TestAggregateRoot.general_format;

public class GeneralTestCreationTests
{
    private readonly AppUserId _creatorId = new AppUserId(Guid.NewGuid());
    private readonly HashSet<AppUserId> _initialEditors = new()
    {
        new AppUserId(Guid.NewGuid()),
        new AppUserId(Guid.NewGuid())
    };

    [Fact]
    public void CreateNew_ShouldCreateTestSuccessfully_WhenValidDataProvided() {
        // Act
        var result = GeneralFormatTest.CreateNew(_creatorId, "Valid Test Name", _initialEditors);

        // Assert
        Assert.True(result.IsSuccess(), "Expected test creation to succeed.");
        var test = result.GetSuccess();
        Assert.True(test.IsUserCreator(_creatorId));
        Assert.Equal(_initialEditors, test.EditorIds);

        var domainEvents = test.GetDomainEventsCopy();
        Assert.Contains(domainEvents, e =>
            e is TestEditorsListChangedEvent ev &&
            ev.NewEditors.SetEquals(_initialEditors) &&
            ev.PreviousEditors.Count == 0);

        Assert.Contains(domainEvents, e =>
            e is NewTestInitializedEvent ev &&
            ev.CreatorId == _creatorId);
    }

    [Fact]
    public void CreateNew_ShouldReturnError_WhenInvalidTestNameProvided() {
        // Arrange
        var invalidTestName = "";

        // Act
        var result = GeneralFormatTest.CreateNew(_creatorId, invalidTestName, _initialEditors);

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Equal(err.Code, Err.ErrCodes.InvalidData);
    }

    [Fact]
    public void CreateNew_ShouldRemoveCreatorFromEditorList_WhenCreatorIsInEditors() {
        // Arrange
        var editorsWithCreator = new HashSet<AppUserId>(_initialEditors) { _creatorId };

        // Act
        var result = GeneralFormatTest.CreateNew(_creatorId, "Test Name", editorsWithCreator);

        // Assert
        Assert.True(result.IsSuccess(), "Expected test creation to succeed even if creator was in editors.");
        var test = result.GetSuccess();
        Assert.DoesNotContain(_creatorId, test.EditorIds);
    }

    [Fact]
    public void CreateNew_ShouldReturnError_WhenEditorCountExceedsLimit() {
        // Arrange
        var excessiveEditors = new HashSet<AppUserId>();
        for (int i = 0; i <= TestRules.MaxTestEditorsCount; i++) {
            excessiveEditors.Add(new AppUserId(Guid.NewGuid()));
        }

        // Act
        var result = GeneralFormatTest.CreateNew(_creatorId, "Test Name", excessiveEditors);

        // Assert
        Assert.True(result.IsErr(out var err));
        Assert.Equal("Too many test editors", err.Message);
    }
}
