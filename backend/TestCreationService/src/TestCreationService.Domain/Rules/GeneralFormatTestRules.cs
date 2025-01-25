using SharedKernel.Common.errors;

namespace TestCreationService.Domain.Rules;

public static class GeneralFormatTestRules
{
    public const int
        MinQuestionsCount = 2,
        MaxQuestionsCount = 60;
    public const int
        MinAnswersCount = 2,
        MaxAnswersCount = 60;
    public const int
        MaxImagesForQuestionCount = 3,
        QuestionTextMinLength = 10,
        QuestionTextMaxLength = 500;


    public static ErrOrNothing CheckQuestionImagesForErrs(IEnumerable<object> images) {
        int count = images?.Count() ?? 0;
        if (count > MaxImagesForQuestionCount) {
            return Err.ErrFactory.InvalidData(
                $"Too many images for the question. Maximum allowed is {MaxImagesForQuestionCount}",
                details: $"Current count: {count}. Maximum allowed: {MaxImagesForQuestionCount}"
            );
        }
        return ErrOrNothing.Nothing;
    }
    public static ErrOrNothing CheckQuestionTextForErrs(string text) {
        int len = string.IsNullOrWhiteSpace(text) ? 0 : text.Length;

        if (len < QuestionTextMinLength) {
            return Err.ErrFactory.InvalidData(
                $"Question text is too short. Minimum length is {QuestionTextMinLength} characters",
                details: $"Current length: {len}. Minimum required: {QuestionTextMinLength}"
            );
        }

        if (len > QuestionTextMaxLength) {
            return Err.ErrFactory.InvalidData(
                $"Question text is too long. Maximum length is {QuestionTextMaxLength} characters",
                details: $"Current length: {len}. Maximum allowed: {QuestionTextMaxLength}"
            );
        }

        return ErrOrNothing.Nothing;
    }
}
