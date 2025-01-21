namespace SharedKernel.Common.tests.value_objects.answers_count_limit;

public abstract class BaseAnswersCountLimit : ValueObject
{
    public bool IsMultipleChoice { get; init; }
    public ushort MinAnswers { get; init; }
    public ushort MaxAnswers { get; init; }

    protected BaseAnswersCountLimit(bool isMultipleChoice, ushort minAnswers, ushort maxAnswers) {
        IsMultipleChoice = isMultipleChoice;
        MinAnswers = minAnswers;
        MaxAnswers = maxAnswers;
    }
    public override IEnumerable<object> GetEqualityComponents()
        => IsMultipleChoice ? [IsMultipleChoice, MinAnswers, MaxAnswers] : [IsMultipleChoice];

    public override string ToString() =>
        IsMultipleChoice ? $"{MinAnswers}-{MaxAnswers}" : "NoCountLimit";
}
