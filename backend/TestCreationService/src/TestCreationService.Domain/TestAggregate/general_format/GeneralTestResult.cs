using SharedKernel.Common;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using System.Collections.Immutable;

namespace TestCreationService.Domain.TestAggregate.general_format;

public class GeneralTestResult : Entity
{
    protected override EntityId EntityId => Id;

    public GeneralTestResultId Id { get; init; }
    public TestId TestId { get; init; }
    public string Name { get; private set; }
    public string? Text { get; private set; }
    public string? Image { get; private set; }
    protected virtual HashSet<GeneralTestAnswerId> _answerLeadingToResultIds { get; init; } = [];
    public ImmutableHashSet<GeneralTestAnswerId> AnswerLeadingToResultIds => _answerLeadingToResultIds
        .Order()
        .ToImmutableHashSet();
    public ErrOrNothing AddAnswerLeadingToResultId(GeneralTestAnswerId answerId) {
        return ErrOrNothing.Nothing;
    }
    public void RemoveAnswerLeadingToResultId(GeneralTestAnswerId answerId) {
        _answerLeadingToResultIds.Remove(answerId);
    }
}
