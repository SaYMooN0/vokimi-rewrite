using ApiShared.interfaces;
using SharedKernel.Common.errors;
using SharedKernel.Common.general_test_questions;
using SharedKernel.Common.general_test_questions.answer_type_specific_data;
using TestCreationService.Domain.Rules;

namespace TestCreationService.Api.Contracts.Tests.test_creation.general_format.questions;

public record class UpdateGeneralFormatTestQuestionRequest(
    string QuestionText,
    string[] Images,
    bool HasTimeLimit,
    int TimeLimitValue,
    bool IsMultipleChoice,
    int MinAnswersCount,
    int MaxAnswersCount
) : IRequestWithValidationNeeded
{
    public RequestValidationResult Validate() {
        ErrList errs = new();
        errs.AddPossibleErr(GeneralTestQuestionRules.CheckQuestionTextForErrs(QuestionText));
        errs.AddPossibleErr(GeneralTestQuestionRules.CheckQuestionImagesForErrs(Images));
        errs.AddPossibleErr(CreateTimeLimit());
        errs.AddPossibleErr(CreateAnswerCountLimit());
        return errs;
    }
    public ErrOr<GeneralTestQuestionTimeLimitOption> CreateTimeLimit() {
        if (!HasTimeLimit) {
            return GeneralTestQuestionTimeLimitOption.NoTimeLimit();
        }
        if (TimeLimitValue > ushort.MaxValue) {
            return Err.ErrFactory.InvalidData(
                $"Incorrect time limit value type",
                details: $"Time limit cannot be more than {ushort.MaxValue} seconds"
            );
        }
        return GeneralTestQuestionTimeLimitOption.HasTimeLimit((ushort)TimeLimitValue);

    }
    public ErrOr<GeneralTestQuestionAnswersCountLimit> CreateAnswerCountLimit() {
        if (!IsMultipleChoice) {
            return GeneralTestQuestionAnswersCountLimit.SingleChoice();
        }
        if (MinAnswersCount < 0 || MinAnswersCount > ushort.MaxValue) {
            return Err.ErrFactory.InvalidData(
                $"Incorrect minimum answers count type",
                details: $"Minimum answers count cannot be less than 0 or more than {ushort.MaxValue}"
            );
        }
        if (MaxAnswersCount < 0 || MaxAnswersCount > ushort.MaxValue) {
            return Err.ErrFactory.InvalidData(
                $"Incorrect maximum answers count type",
                details: $"Maximum answers count cannot be less than 0 or more than {ushort.MaxValue}"
            );
        }
        return GeneralTestQuestionAnswersCountLimit.MultipleChoice((ushort)MinAnswersCount, (ushort)MaxAnswersCount);
    }
}

