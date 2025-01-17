
using SharedKernel.Common.tests;

namespace TestCreationService.Domain.Rules;

public static class GeneralFormatTestRules
{
    public static readonly GeneralTestAnswersType[] TimeLimitedQuestionsNotAllowedAnswers = [
        GeneralTestAnswersType.AudioOnly, 
        GeneralTestAnswersType.TextAndAudio
    ];
}
