using SharedKernel.Common.domain.entity;

namespace TestTakingService.Domain.TestAggregate.general_format;

public class GeneralTestResult : Entity<GeneralTestResultId>
{
    private GeneralTestResult() { }

    public GeneralTestResult(
        GeneralTestResultId resultId,
        string name,
        string text,
        string image
    ) {
        Id = resultId;
        Name = name;
        Text = text;
        Image = image;
    }

    public string Name { get; init; }
    public string Text { get; init; }
    public string Image { get; init; }
}