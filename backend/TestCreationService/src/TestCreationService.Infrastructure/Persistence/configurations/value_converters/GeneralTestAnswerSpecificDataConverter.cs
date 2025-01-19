using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel.Common.tests.general_format_tests;
using System.Text.Json;
using static SharedKernel.Common.tests.general_format_tests.GeneralTestAnswerTypeSpecificData;

namespace TestCreationService.Infrastructure.Persistence.configurations.value_converters;

internal class GeneralTestAnswerSpecificDataConverter : ValueConverter<GeneralTestAnswerTypeSpecificData, string>
{
    public GeneralTestAnswerSpecificDataConverter() : base(
        (val) => AnswerSpecificDataConverterToDbString(val),
        (str) => DbStringToAnswerSpecificDataConverter(str)
    ) { }
    private static string AnswerSpecificDataConverterToDbString(GeneralTestAnswerTypeSpecificData value) =>
        value.MatchingEnumType.ToString() + ':' + value.MatchingEnumType switch {
            GeneralTestAnswersType.TextOnly => JsonSerializer.Serialize(value as TextOnly),
            GeneralTestAnswersType.ImageOnly => JsonSerializer.Serialize(value as ImageOnly),
            GeneralTestAnswersType.ImageAndText => JsonSerializer.Serialize(value as ImageAndText),
            GeneralTestAnswersType.ColorOnly => JsonSerializer.Serialize(value as ColorOnly),
            GeneralTestAnswersType.ColorAndText => JsonSerializer.Serialize(value as ColorAndText),
            GeneralTestAnswersType.AudioOnly => JsonSerializer.Serialize(value as AudioOnly),
            GeneralTestAnswersType.AudioAndText => JsonSerializer.Serialize(value as AudioAndText),
            _ => throw new ArgumentOutOfRangeException(
                $"Unsupported enum type: {value.MatchingEnumType} in " +
                $"{nameof(GeneralTestAnswerSpecificDataConverter)} {nameof(AnswerSpecificDataConverterToDbString)}"
            )
        };
    private static GeneralTestAnswerTypeSpecificData DbStringToAnswerSpecificDataConverter(string str) {
        var split = str.Split(':', 2);
        var matchingEnumType = Enum.Parse<GeneralTestAnswersType>(split[0]);
        return matchingEnumType switch {
            GeneralTestAnswersType.TextOnly => JsonSerializer.Deserialize<TextOnly>(split[1])!,
            GeneralTestAnswersType.ImageOnly => JsonSerializer.Deserialize<ImageOnly>(split[1])!,
            GeneralTestAnswersType.ImageAndText => JsonSerializer.Deserialize<ImageAndText>(split[1])!,
            GeneralTestAnswersType.ColorOnly => JsonSerializer.Deserialize<ColorOnly>(split[1])!,
            GeneralTestAnswersType.ColorAndText => JsonSerializer.Deserialize<ColorAndText>(split[1])!,
            GeneralTestAnswersType.AudioOnly => JsonSerializer.Deserialize<AudioOnly>(split[1])!,
            GeneralTestAnswersType.AudioAndText => JsonSerializer.Deserialize<AudioAndText>(split[1])!,
            _ => throw new ArgumentOutOfRangeException(
                $"Unsupported enum type: {matchingEnumType} in " +
                $"{nameof(GeneralTestAnswerSpecificDataConverter)} {nameof(DbStringToAnswerSpecificDataConverter)}"
            )
        };
    }
}