using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel.Common.domain.entity;
using TestTakingService.Domain.Common.test_taken_data.tier_list_format_test;

namespace TestTakingService.Infrastructure.Persistence.configurations.value_converters;

internal class TierListTestTakenTierDataConverter : ValueConverter<TierListTestTakenTierData, string>
{
    public TierListTestTakenTierDataConverter() : base(
        v => ToString(v),
        v => FromString(v)
    ) { }
    private static string ToString(TierListTestTakenTierData data) =>
        string.Join(",", data.Select(kvp => $"{kvp.Key.Value}:{kvp.Value}"));

    private static TierListTestTakenTierData FromString(string value)
    {
        var result = new TierListTestTakenTierData();

        if (string.IsNullOrWhiteSpace(value))
            return result;

        foreach (var part in value.Split(',', StringSplitOptions.RemoveEmptyEntries))
        {
            var parts = part.Split(':');
            if (parts.Length != 2)
                continue;

            var id = new TierListTestItemId(new(parts[0]));
            var order = ushort.Parse(parts[1]);

            result[id] = order;
        }

        return result;
    }
}