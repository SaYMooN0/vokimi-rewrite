using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity_id;
using SharedKernel.Common.errors;
using TestCreationService.Domain.Rules;
using TestCreationService.Domain.TestAggregate.general_format;
using TestCreationService.Domain.TestAggregate.general_format.events;

namespace TestCreationService.Domain.UnitTests.TestAggregateRoot.general_format;

public class GeneralTestResultsTests
{
    private readonly AppUserId _creatorId = new(Guid.NewGuid());
    private GeneralFormatTest CreateTest() => GeneralFormatTest.CreateNew(_creatorId, "Test Name", []).GetSuccess();

    [Fact]
    public void GetTestResultIdsWithNames_ShouldReturnCorrectResultIdsWithNames() {
        // Arrange
        var test = CreateTest();

        // Add some results
        test.CreateResult();
        test.CreateResult();

        // Act
        var result = test.GetTestResultIdsWithNames();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, r => r.Value == "Test result #1");
        Assert.Contains(result, r => r.Value == "Test result #2");
    }

    [Fact]
    public void CreateResult_ShouldCreateNewResult_WhenValidDataProvided() {
        // Arrange
        var test = CreateTest();

        // Act
        var result = test.CreateResult();

        // Assert
        Assert.True(result.IsSuccess(), "Expected result creation to succeed.");
        var createdResult = result.GetSuccess();
        Assert.Equal("Test result #1", createdResult.Name);
        Assert.Contains(test.GetTestResultIdsWithNames(), r => r.Value == "Test result #1");
    }

    [Fact]
    public void CreateResult_ShouldReturnError_WhenMaxResultsReached() {
        // Arrange
        var test = CreateTest();

        // Add the maximum number of results
        for (int i = 0; i < GeneralFormatTestRules.MaxResultsInTestCount + 1; i++) {
            test.CreateResult();
        }

        // Act
        var result = test.CreateResult();

        // Assert
        Assert.True(result.IsErr(out var err), "Expected error when trying to create more than max results.");
    }

    [Fact]
    public void DeleteResult_ShouldRemoveResult_WhenResultExists() {
        // Arrange
        var test = CreateTest();
        var result = test.CreateResult().GetSuccess();

        // Act
        var deleteResult = test.DeleteResult(result.Id);

        // Assert
        Assert.True(!deleteResult.IsErr(), "Expected result deletion to succeed.");
        Assert.Empty(test.GetTestResultIdsWithNames());
        Assert.Contains(test.GetDomainEventsCopy(), e =>
            e is GeneralTestResultDeletedEvent ev && ev.ResultId == result.Id
        );
    }

    [Fact]
    public void DeleteResult_ShouldReturnError_WhenResultDoesNotExist() {
        // Arrange
        var test = CreateTest();

        var nonExistentResultId = new GeneralTestResultId(Guid.NewGuid());

        // Act
        var deleteResult = test.DeleteResult(nonExistentResultId);

        // Assert
        Assert.True(deleteResult.IsErr(out var err), "Expected error when trying to delete a non-existent result.");
        Assert.Equal(err.Code, Err.ErrCodes.NotFound);
    }

    [Fact]
    public void UpdateResult_ShouldUpdateResult_WhenValidDataProvided() {
        // Arrange
        var test = CreateTest();
        var result = test.CreateResult().GetSuccess();

        string newName = "Updated Result Name";
        string newText = "Updated Text";
        string newImage = "Updated Image";

        // Act
        var updateResult = test.UpdateResult(result.Id, newName, newText, newImage);

        // Assert
        Assert.True(!updateResult.IsErr(), "Expected result update to succeed.");
        var updatedResult = test.GetTestResultIdsWithNames()[result.Id];
        Assert.Equal(newName, updatedResult);
    }

    [Fact]
    public void UpdateResult_ShouldReturnError_WhenResultNameNotUnique() {
        // Arrange
        var test = CreateTest();
        var firstResult = test.CreateResult().GetSuccess();
        var secondResult = test.CreateResult().GetSuccess();

        // Attempt to update the second result with the name of the first
        string newName = firstResult.Name;

        // Act
        var updateResult = test.UpdateResult(secondResult.Id, newName, "Some Text", "Some Image");

        // Assert
        Assert.True(updateResult.IsErr(out var err), "Expected error when trying to use a non-unique result name.");
        Assert.Equal(err.Code, Err.ErrCodes.InvalidData);
    }

    [Fact]
    public void UpdateResult_ShouldReturnError_WhenResultNotFound() {
        // Arrange
        var test = CreateTest();
        var nonExistentResultId = new GeneralTestResultId(Guid.NewGuid());

        // Act
        var updateResult = test.UpdateResult(nonExistentResultId, "New Name", "New Text", "New Image");

        // Assert
        Assert.True(updateResult.IsErr(out var err), "Expected error when trying to update a non-existent result.");
        Assert.Equal(err.Code, Err.ErrCodes.NotFound);
    }
}