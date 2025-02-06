using SharedKernel.Common.domain;

namespace TestTakingService.Domain.TestAggregate.general_format;

public class GeneralTestResult : Entity<GeneralTestResultId>
{
    private GeneralTestResult() { }
    private TestId _testId { get; init; }
    public string Name { get; init; }
    public string Text { get; init; }
    public string Image { get; init; }

    public GeneralTestResult(
        GeneralTestResultId resultId,
        TestId testId,
        string name,
        string text,
        string image
    ) {
        Id = resultId;
        _testId = testId;
        Name = name;
        Text = text;
        Image = image;
    }
}