using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel.Common.general_test_questions.answer_type_specific_data;

namespace TestCreationService.Infrastructure.Persistence.configurations.value_converters.general_format_test;

internal class GeneralTestQuestionAnswersCountLimitConverter : ValueConverter<GeneralTestQuestionAnswersCountLimit, string>
{
    public GeneralTestQuestionAnswersCountLimitConverter() : base(
        v => v.ToString(),
        v => FromString(v)
    ) {
    }
    private static GeneralTestQuestionAnswersCountLimit FromString(string value) {
        var res = GeneralTestQuestionAnswersCountLimit.FromString(value);
        if (res.IsErr(out var err)) {
            throw new ArgumentException($"Incorrect time limit value in the data base: {err}");
        }
        return res.GetSuccess();
    }
}