using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel.Common.tests.value_objects;

namespace TestCreationService.Infrastructure.Persistence.configurations.value_converters;

internal class AnswerCountLimitConverter : ValueConverter<AnswerCountLimit, string>
{
    public AnswerCountLimitConverter() : base(
        v => v.IsMultipleChoice ? v.MinAnswers.ToString() + '-' + v.MaxAnswers.ToString() : "NoCountLimit",
        v => v == "NoCountLimit"
            ? AnswerCountLimit.SingleChoice()
            : AnswerCountLimit.MultipleChoice(
                ushort.Parse(v.Split('-', StringSplitOptions.RemoveEmptyEntries)[0]), 
                ushort.Parse(v.Split('-', StringSplitOptions.RemoveEmptyEntries)[1])
            )
    ) {
    }
}