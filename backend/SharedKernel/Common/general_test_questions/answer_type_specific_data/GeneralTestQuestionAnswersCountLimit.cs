using SharedKernel.Common.errors;
using SharedKernel.Common.tests.value_objects;

namespace SharedKernel.Common.general_test_questions.answer_type_specific_data;


public sealed class GeneralTestQuestionAnswersCountLimit : BaseAnswersCountLimit
{
    private const ushort MinPossibleAnswersLimit = 1;

    private GeneralTestQuestionAnswersCountLimit(bool isMultipleChoice, ushort minAnswers, ushort maxAnswers)
        : base(isMultipleChoice, minAnswers, maxAnswers) { }

    public static GeneralTestQuestionAnswersCountLimit SingleChoice()
        => new GeneralTestQuestionAnswersCountLimit(false, 1, 1);

    public static ErrOr<GeneralTestQuestionAnswersCountLimit> MultipleChoice(ushort minAnswers, ushort maxAnswers) {
        if (minAnswers < MinPossibleAnswersLimit) {
            return Err.ErrFactory.InvalidData(
                message: "Minimum answers value is less than allowed.",
                details: $"Provided value '{minAnswers}' is less than the minimum allowed of {MinPossibleAnswersLimit}."
            );
        }
        if (minAnswers > maxAnswers) {
            return Err.ErrFactory.InvalidData(
                message: "Invalid range for answers.",
                details: $"Minimum answers '{minAnswers}' cannot be greater than maximum answers '{maxAnswers}'."
            );
        }

        return new GeneralTestQuestionAnswersCountLimit(true, minAnswers, maxAnswers);
    }

    public static ErrOr<GeneralTestQuestionAnswersCountLimit> FromString(string value) {
        if (value == "NoCountLimit") {
            return SingleChoice();
        }

        string[] parts = value.Split('-');
        if (parts.Length != 2) {
            return Err.ErrFactory.InvalidData(
                message: "Invalid format for Answer Count Limit.",
                details: "Expected format 'min-max' with two values separated by '-' or 'NoCountLimit' value."
            );
        }

        if (!ushort.TryParse(parts[0], out var min)) {
            return Err.ErrFactory.InvalidData(
                message: "Invalid minimum answer count.",
                details: $"The value '{parts[0]}' cannot be parsed as a valid number for the minimum answers."
            );
        }

        if (!ushort.TryParse(parts[1], out var max)) {
            return Err.ErrFactory.InvalidData(
                message: "Invalid maximum answer count.",
                details: $"The value '{parts[1]}' cannot be parsed as a valid number for the maximum answers."
            );
        }

        return MultipleChoice(min, max);
    }
}
