
namespace TestCreationService.Domain.TestAggregate.formats_shared;

public class TestPublishingProblem
{
    public TestPublishingProblem(string area, string message) {
        Area = area;
        Message = message;
    }

    public string Area { get; init; }
    public string Message { get; init; }
}
