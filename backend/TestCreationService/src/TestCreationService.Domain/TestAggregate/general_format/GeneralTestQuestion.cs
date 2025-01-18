
using SharedKernel.Common;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests.general_format_tests;
using SharedKernel.Common.tests.value_objects;
using TestCreationService.Domain.Common.contracts.general_tests;
using TestCreationService.Domain.Common.rules;

namespace TestCreationService.Domain.TestAggregate.general_format;

public class GeneralTestQuestion : Entity
{
    protected override EntityId EntityId => Id;

    public GeneralTestQuestionId Id { get; init; }
    public TestId TestId { get; init; }
    public string Text { get; init; }
    private List<string> _images { get; init; }
    public IReadOnlyList<string> Images => _images.AsReadOnly();
    public TimeLimitOption TimeLimit { get; private set; }
    public GeneralTestAnswersType AnswersType { get; init; }
    protected virtual List<GeneralTestAnswer> _answers { get; set; } = [];
    public IReadOnlyList<GeneralTestAnswer> Answers => _answers
        .OrderBy(a => _answersOrderDictionary.TryGetValue(a.Id, out var order) ? order : ushort.MaxValue)
        .ToList()
        .AsReadOnly();
    private Dictionary<GeneralTestAnswerId, ushort> _answersOrderDictionary { get; set; } = [];
    public AnswerCountLimit AnswerCountLimit { get; private set; }

    public static GeneralTestQuestion CreateNew(TestId testId, GeneralTestAnswersType answersType) => new() {
        Id = GeneralTestQuestionId.CreateNew(),
        Text = "New question",
        _images = [],
        TimeLimit = TimeLimitOption.NoTimeLimit(),
        AnswersType = answersType,
        _answers = [],
        _answersOrderDictionary = [],
        AnswerCountLimit = AnswerCountLimit.SingleChoice(),
    };
    public static ErrListOrNothing Update(UpdateGeneralTestQuestionDto dto) {
        return Err.ErrFactory.NotImplemented();
        //foreach answer .Update
    }
    public ErrOrNothing AddTimeLimit(ushort seconds) {
        if (seconds > TestRules.MaxQuestionTimeLimitSeconds) {
            return new Err($"Time limit can't be greater than {TestRules.MaxQuestionTimeLimitSeconds} seconds.");
        }
        if (seconds < TestRules.MinQuestionTimeLimitSeconds) {
            return new Err($"Time limit can't be less than {TestRules.MinQuestionTimeLimitSeconds} seconds.");
        }
        TimeLimit = TimeLimitOption.HasTimeLimit(seconds);
        return ErrOrNothing.Nothing;
    }
    public void RemoveTimeLimit() {
        TimeLimit = TimeLimitOption.NoTimeLimit();
    }
}
