using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests.tier_list_format.items;

namespace InfrastructureConfigurationShared.ValueConverters.tier_list_format_test;

internal class TierListTestItemContentDataConverter: ValueConverter<TierListTestItemContentData, string>
{
    public TierListTestItemContentDataConverter() : base(
        (val) => ItemContentDataConverterToDbString(val),
        (str) => DbStringToItemContentDataConverter(str)
    ) { }

    private static string ItemContentDataConverterToDbString(TierListTestItemContentData value) =>
        value.MatchingEnumType.ToString() + ':' + JsonSerializer.Serialize(value.ToDictionary());

    private static TierListTestItemContentData DbStringToItemContentDataConverter(string str) {
        var split = str.Split(':', 2);
        var matchingEnumType = Enum.Parse<TierListTestItemContentType>(split[0]);
        var dictionary = JsonSerializer.Deserialize<Dictionary<string, string>>(split[1]);
        if (dictionary is null) {
            throw new ErrCausedException(
                new Err($"Unable to parse {nameof(TierListTestItemContentData)} from db")
            );
        }

        var parseRes = TierListTestItemContentData.CreateFromDictionary(matchingEnumType, dictionary);

        if (parseRes.IsErr(out var err)) {
            throw new ErrCausedException(err);
        }

        return parseRes.GetSuccess();
    }
}