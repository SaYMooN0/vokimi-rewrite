using System.Text.Json.Serialization;
using SharedKernel.Common.domain.value_object;
using SharedKernel.Common.errors;

namespace SharedKernel.Common.tests.tier_list_format;

public abstract partial class TierListTestItemContentData : ValueObject
{
    [JsonIgnore]
    public abstract TierListTestItemContentType MatchingEnumType { get; }
    public abstract Dictionary<string, string> ToDictionary();
    public static ErrOr<TierListTestItemContentData> CreateFromDictionary(
        TierListTestItemContentType type,
        Dictionary<string, string> dictionary
    ) => type switch {
      
        _ => throw new ArgumentException($"Unsupported answer type {type} in the {nameof(CreateFromDictionary)}")
    };
}