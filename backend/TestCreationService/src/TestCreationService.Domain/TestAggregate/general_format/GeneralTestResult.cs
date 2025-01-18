using SharedKernel.Common;
using SharedKernel.Common.EntityIds;

namespace TestCreationService.Domain.TestAggregate.general_format;

public class GeneralTestResult : Entity
{
    protected override EntityId EntityId => Id;

    public GeneralTestResultId Id { get; init; }
    public TestId TestId { get; init; }
    public string Name { get; private set; }
    public string? Text { get; private set; }
    public string? Image { get; private set; }
    protected virtual List<GeneralTestAnswer> _answersLeadingToResult { get; init; } = [];
    public IReadOnlyList<GeneralTestAnswer> AnswersLeadingToResult => _answersLeadingToResult
        .OrderBy(a=>a.Id)
        .ToList()
        .AsReadOnly();
}
