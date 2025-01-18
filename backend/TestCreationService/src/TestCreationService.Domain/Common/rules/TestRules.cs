namespace TestCreationService.Domain.Common.rules;

public static class TestRules
{
    public const int
        MinNameLength = 8,
        MaxNameLength = 255;
    public const int MaxTestEditorsCount = 10;

    public const int MaxTestDescriptionLength = 511;
    public const int
        MinQuestionTimeLimitSeconds = 5,
        MaxQuestionTimeLimitSeconds = 600;
}
