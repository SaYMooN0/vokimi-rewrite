using System.Text.Json.Serialization;
using SharedKernel.Common.domain.value_object;
using SharedKernel.Common.errors;

namespace SharedKernel.Common.tests.tier_list_format.items;

public abstract partial class TierListTestItemContentData : ValueObject
{
    [JsonIgnore] public abstract TierListTestItemContentType MatchingEnumType { get; }
    public abstract Dictionary<string, string> ToDictionary();

    public static ErrOr<TierListTestItemContentData> CreateFromDictionary(
        TierListTestItemContentType type,
        Dictionary<string, string> dictionary
    ) => type switch {
        TierListTestItemContentType.TextOnly => TextOnly.CreateFromDictionary(dictionary)
            .Match(ErrOr<TierListTestItemContentData>.Success, err => err),
        TierListTestItemContentType.ImageOnly => ImageOnly.CreateFromDictionary(dictionary)
            .Match(ErrOr<TierListTestItemContentData>.Success, err => err),
        TierListTestItemContentType.ImageAndText => ImageAndText.CreateFromDictionary(dictionary)
            .Match(ErrOr<TierListTestItemContentData>.Success, err => err),
        TierListTestItemContentType.AudioOnly => AudioOnly.CreateFromDictionary(dictionary)
            .Match(ErrOr<TierListTestItemContentData>.Success, err => err),
        TierListTestItemContentType.ColorOnly => ColorOnly.CreateFromDictionary(dictionary)
            .Match(ErrOr<TierListTestItemContentData>.Success, err => err),
        _ => throw new ArgumentException($"Unsupported content type {type} in the {nameof(CreateFromDictionary)}")
    };
}