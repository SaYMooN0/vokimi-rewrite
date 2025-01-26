using SharedKernel.Common.errors;

namespace TestCreationService.Domain.Rules;

public static class GeneralFormatTestRules
{
    public const int
        MinQuestionsCount = 2,
        MaxQuestionsCount = 60;
    public const int
        MaxRelatedResultsForAnswer = 20;
}
