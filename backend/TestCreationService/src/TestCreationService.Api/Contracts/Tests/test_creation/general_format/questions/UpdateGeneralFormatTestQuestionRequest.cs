using ApiShared.interfaces;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests.value_objects;
using TestCreationService.Domain.Common.rules;

namespace TestCreationService.Api.Contracts.Tests.test_creation.general_format.questions;

public record class UpdateGeneralFormatTestQuestionRequest(
    string QuestionId,
    string Text,
    string[] Images,
    bool HasTimeLimit,
    int TimeLimitValue,
    bool IsMultipleChoice,
    int MinAnswersCount,
    int MaxAnswersCount
) : IRequestWithValidationNeeded
{
    public RequestValidationResult Validate() {
        if (!Guid.TryParse(QuestionId, out _)) {
            return new Err(
                message: "Unable to parse question id",
                details: "Question id is not a valid format"
            );
        }
        ErrList errs = new();

        int textLen = string.IsNullOrWhiteSpace(Text) ? 0 : Text.Length;
        if (textLen < GeneralFormatTestRules.QuestionTextMinLength) {
            errs.Add(Err.ErrFactory.InvalidData(
                $"Text length is too short. It must be at least {GeneralFormatTestRules.QuestionTextMinLength}",
                details: $"Current text length is {textLen}")
            );
        } else if (textLen > GeneralFormatTestRules.QuestionTextMaxLength) {
            errs.Add(Err.ErrFactory.InvalidData(
                $"Text length is too long. It must be at most {GeneralFormatTestRules.QuestionTextMaxLength}",
                details: $"Current text length is {textLen}")
            );
        }

        if (HasTimeLimit && TimeLimitValue < GeneralFormatTestRules.MinQuestionTimeLimitSeconds) {
            errs.Add(Err.ErrFactory.InvalidData(
                $"Time limit is too small. It must be at least {GeneralFormatTestRules.MinQuestionTimeLimitSeconds} seconds",
                details: $"Current time limit is {TimeLimitValue} seconds")
            );
        }
        if (HasTimeLimit && TimeLimitValue > GeneralFormatTestRules.MaxQuestionTimeLimitSeconds) {
            errs.Add(Err.ErrFactory.InvalidData(
                $"Time limit is too big. It must be at most {GeneralFormatTestRules.MaxQuestionTimeLimitSeconds} seconds",
                details: $"Current time limit is {TimeLimitValue} seconds")
            );
        }

        if (IsMultipleChoice && MinAnswersCount < 1) {
            errs.Add(Err.ErrFactory.InvalidData($"Incorrect minimum answers count. It must be at least 1"));
        }

        if (errs.Any()) { return errs; }
        return RequestValidationResult.Success;
    }
}

