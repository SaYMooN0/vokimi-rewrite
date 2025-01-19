using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel.Common.tests.value_objects;

namespace TestCreationService.Infrastructure.Persistence.configurations.value_converters;

internal class AnswerCountLimitConverter : ValueConverter<AnswerCountLimit, string>
{
    public AnswerCountLimitConverter() : base(
        v => v.ToString(),
        v => FromString(v)
    ) {
    }
    private static AnswerCountLimit FromString(string value) {
        var res = AnswerCountLimit.FromString(value);
        if (res.IsErr(out var err)) {
            throw new ArgumentException($"Incorrect time limit value in the data base: {err}");
        }
        return res.GetSuccess();
    }
}