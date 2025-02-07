using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel.Common.errors;
using SharedKernel.Common.general_test_questions;
using SharedKernel.Common.general_test_questions.answer_type_specific_data;

namespace InfrastructureConfigurationShared.ValueConverters.general_format_test;

internal class GeneralTestAnswerSpecificDataConverter : ValueConverter<GeneralTestAnswerTypeSpecificData, string>
{
    public GeneralTestAnswerSpecificDataConverter() : base(
        (val) => AnswerSpecificDataConverterToDbString(val),
        (str) => DbStringToAnswerSpecificDataConverter(str)
    ) { }
    private static string AnswerSpecificDataConverterToDbString(GeneralTestAnswerTypeSpecificData value) =>
        value.MatchingEnumType.ToString() + ':' + JsonSerializer.Serialize(value.ToDictionary());
    private static GeneralTestAnswerTypeSpecificData DbStringToAnswerSpecificDataConverter(string str) {
        var split = str.Split(':', 2);
        var matchingEnumType = Enum.Parse<GeneralTestAnswersType>(split[0]);
        var dictionary = JsonSerializer.Deserialize<Dictionary<string, string>>(split[1]);
        if (dictionary is null) {
            throw new ErrCausedException(new Err("Unable to parse General Test Answer Type Specific Data from db"));
        }
        var parseRes = GeneralTestAnswerTypeSpecificData.CreateFromDictionary(matchingEnumType, dictionary);

        if (parseRes.IsErr(out var err)) {
            throw new ErrCausedException(err);
        }
        return parseRes.GetSuccess();
    }
}