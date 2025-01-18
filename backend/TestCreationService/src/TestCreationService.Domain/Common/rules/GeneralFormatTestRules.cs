using SharedKernel.Common.tests.general_format_tests;

namespace TestCreationService.Domain.Common.rules;

public static class GeneralFormatTestRules
{
    public const int 
        MinQuestionsCount = 2,
        MaxQuestionsCount = 60;
    public const int
        MinAnswersCount = 2,
        MaxAnswersCount = 60;
    public const int
        QuestionTextMinLength = 10,
        QuestionTextMaxLength = 500;
    public const int
        TexOnlyAnswerMinLength = 5,
        TexOnlyAnswerMaxLength = 500,
        AdditionalTextMinLength = 5,
        AdditionalTextMaxLength = 250;
}
