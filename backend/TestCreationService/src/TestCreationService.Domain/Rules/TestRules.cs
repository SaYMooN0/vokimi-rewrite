namespace TestCreationService.Domain.Rules;

public static class TestRules
{
    public const int
        MinNameLength = 8,
        MaxNameLength = 255;
    public const int MaxTestEditorsCount = 10;

    public const int MaxTestDescriptionLength = 511;
}
