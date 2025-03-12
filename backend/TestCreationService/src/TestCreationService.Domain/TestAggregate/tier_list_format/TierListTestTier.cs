using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using TestCreationService.Domain.Rules;

namespace TestCreationService.Domain.TestAggregate.tier_list_format;

public class TierListTestTier : Entity<TierListTestTierId>
{
    private TierListTestTier() { }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public ushort? ItemsLimit { get; private set; } //if null => disabled

    public static ErrOr<TierListTestTier> CreateNew(string name) {
        if (TierListTestTiersRules.CheckTierNameForErrs(name).IsErr(out var err)) {
            return err;
        }

        return new TierListTestTier() {
            Name = name,
            Description = null,
            ItemsLimit = null,
        };
    }
}