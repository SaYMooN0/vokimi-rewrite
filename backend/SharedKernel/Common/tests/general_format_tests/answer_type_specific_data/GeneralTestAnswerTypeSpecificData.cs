using SharedKernel.Common.errors;
using System.Text.Json.Serialization;

namespace SharedKernel.Common.tests.general_format_tests.answer_type_specific_data;
public abstract partial class GeneralTestAnswerTypeSpecificData : ValueObject
{
    [JsonIgnore]
    public abstract GeneralTestAnswersType MatchingEnumType { get; }
    public abstract Dictionary<string, string> ToDictionary();
    public static ErrOr<GeneralTestAnswerTypeSpecificData> CreateFromDictionary(
        GeneralTestAnswersType type,
        Dictionary<string, string> dictionary
    ) => type switch {
        GeneralTestAnswersType.TextOnly => TextOnly.CreateFromDictionary(dictionary)
            .Match(ErrOr<GeneralTestAnswerTypeSpecificData>.Success, err => err),

        GeneralTestAnswersType.ImageOnly => ImageOnly.CreateFromDictionary(dictionary)
            .Match(ErrOr<GeneralTestAnswerTypeSpecificData>.Success, err => err),

        GeneralTestAnswersType.ImageAndText => ImageAndText.CreateFromDictionary(dictionary)
            .Match(ErrOr<GeneralTestAnswerTypeSpecificData>.Success, err => err),

        GeneralTestAnswersType.ColorOnly => ColorOnly.CreateFromDictionary(dictionary)
            .Match(ErrOr<GeneralTestAnswerTypeSpecificData>.Success, err => err),

        GeneralTestAnswersType.ColorAndText => ColorAndText.CreateFromDictionary(dictionary)
            .Match(ErrOr<GeneralTestAnswerTypeSpecificData>.Success, err => err),

        GeneralTestAnswersType.AudioOnly => AudioOnly.CreateFromDictionary(dictionary)
            .Match(ErrOr<GeneralTestAnswerTypeSpecificData>.Success, err => err),

        GeneralTestAnswersType.AudioAndText => AudioAndText.CreateFromDictionary(dictionary)
            .Match(ErrOr<GeneralTestAnswerTypeSpecificData>.Success, err => err),

        _ => throw new ArgumentException($"Unsupported answer type {type} in the {nameof(CreateFromDictionary)}")
    };
}
