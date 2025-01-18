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

    public static AnswerCountLimit MultipleChoice(ushort minAnswers, ushort maxAnswers) => new(true, minAnswers, maxAnswers);

    public override IEnumerable<object> GetEqualityComponents()
        => IsMultipleChoice ? [ IsMultipleChoice, MinAnswers, MaxAnswers ] : [IsMultipleChoice ];
}
