using SharedKernel.Common.errors;

namespace SharedKernel.Common.tests.value_objects;

public class AnswerCountLimit : ValueObject
{
    public bool IsMultipleChoice { get; init; }
    public ushort MinAnswers { get; init; }
    public ushort MaxAnswers { get; init; }

    public AnswerCountLimit(bool isMultipleChoice, ushort minAnswers, ushort maxAnswers) {
        IsMultipleChoice = isMultipleChoice;
        if (isMultipleChoice) {
            MinAnswers = minAnswers;
            MaxAnswers = maxAnswers;
        } else {
            MinAnswers = 1;
            MaxAnswers = 1;
        }
    }

    public static AnswerCountLimit SingleChoice() => new(false, 1, 1);

    public static ErrOr<AnswerCountLimit> MultipleChoice(ushort minAnswers, ushort maxAnswers) {
        if (minAnswers > maxAnswers) {
            return new Err(
                $"Minimum answers cannot be greater than maximum answers.",
                details: $" Given values: min:'{minAnswers}' and max:'{maxAnswers}'"
            );
        }
        return new AnswerCountLimit(true, minAnswers, maxAnswers);
    }

    public override IEnumerable<object> GetEqualityComponents()
        => IsMultipleChoice ? [IsMultipleChoice, MinAnswers, MaxAnswers] : [IsMultipleChoice];
    public static ErrOr<AnswerCountLimit> FromString(string value) {
        if (value == "NoCountLimit") { return SingleChoice(); }

        string[] parts = value.Split('-');
        if (parts.Length != 2) {
            return new Err(
                $"Invalid format for Answer Count Limit: '{value}'",
                details: "Expected format 'min-max' with two values separated by '-' or 'NoCountLimit' value"
            );
        }
        if (!ushort.TryParse(parts[0], out var min) || !ushort.TryParse(parts[1], out var max)) {
            return new Err(
                $"Invalid format for AnswerCountLimit: '{value}'",
                details: $"Unable to parse min and max values from string. Given values: '{parts[0]}' and '{parts[1]}'");
        }

        return MultipleChoice(min, max);
    }

    public override string ToString() =>
        IsMultipleChoice ? $"{MinAnswers}-{MaxAnswers}" : "NoCountLimit";
}
