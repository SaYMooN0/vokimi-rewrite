
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests;
using SharedKernel.Common.tests.value_objects;
using TestCreationService.Domain.Rules;

namespace TestCreationService.Domain.TestAggregate.general_format;

public class GeneralTestQuestion
{
    public GeneralTestQuestionId Id { get; init; }
    public TestId TestId { get; init; }
    public string Text { get; init; }
    public string[] Images { get; init; }
    public GeneralTestAnswersType AnswersType { get; init; }
    public virtual ICollection<GeneralTestAnswer> Answers { get; protected set; } = [];
    // possible multiple answers
    public TimeLimitOption TimeLimit { get; private set; }
    public ErrOrNothing ChangeTimeLimit(ushort seconds) {
        if (TimeLimit.TimeLimitExists == false) {
            return new Err("Cannot change time limit for the question because it was created without time limit.");
        }
        if (seconds > TestRules.MaxQuestionTimeLimitSeconds) {
            return new Err($"Time limit can't be greater than {TestRules.MaxQuestionTimeLimitSeconds} seconds.");
        }
        if (seconds < TestRules.MinQuestionTimeLimitSeconds) {
            return new Err($"Time limit can't be less than {TestRules.MinQuestionTimeLimitSeconds} seconds.");
        }
        TimeLimit = TimeLimitOption.HasTimeLimit(seconds);
        return ErrOrNothing.Nothing;
    }
}
