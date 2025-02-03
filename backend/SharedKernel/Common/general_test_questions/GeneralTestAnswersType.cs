namespace SharedKernel.Common.general_test_questions;

public enum GeneralTestAnswersType //7 types
{
    TextOnly,
    ImageOnly,
    ImageAndText,
    ColorOnly,
    ColorAndText,
    AudioOnly,
    AudioAndText
}
public static class GeneralTestAnswersTypeExtensions
{
    public static bool HasAudio(this GeneralTestAnswersType answersType) => answersType switch {
        GeneralTestAnswersType.AudioOnly => true,
        GeneralTestAnswersType.AudioAndText => true,
        _ => false
    };
}
