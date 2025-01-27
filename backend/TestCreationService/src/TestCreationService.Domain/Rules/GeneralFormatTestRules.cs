using SharedKernel.Common.errors;

namespace TestCreationService.Domain.Rules;

public static class GeneralFormatTestRules
{
    public const int
        MinQuestionsCount = 2,
        MaxQuestionsCount = 60;
    public const int
        MaxRelatedResultsForAnswer = 20;
    public const int
        MinResultsInTestCount = 60,
        MaxResultsInTestCount = 60;
    public const int
        MinResultNameLength = 6,
        MaxResultNameLength = 60;
    public static ErrOrNothing CheckResultNameForErrs(string name) {
        int len = string.IsNullOrWhiteSpace(name) ? 0 : name.Length;
        if (len < MinResultNameLength) {
            return Err.ErrFactory.InvalidData(
                $"Result name is too short. Minimum length is {MinResultNameLength} characters",
                details: $"Current length: {len}. Minimum required: {MinResultNameLength}"
            );
        }
        if (len > MaxResultNameLength) {
            return Err.ErrFactory.InvalidData(
                $"Result name is too long. Maximum length is {MaxResultNameLength} characters",
                details: $"Current length: {len}. Maximum allowed: {MaxResultNameLength}"
            );
        }
        return ErrOrNothing.Nothing;
    }
}
