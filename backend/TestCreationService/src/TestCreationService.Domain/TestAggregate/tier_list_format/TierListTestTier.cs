using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests.tier_list_format;
using SharedKernel.IntegrationEvents.test_publishing;
using TestCreationService.Domain.Rules;

namespace TestCreationService.Domain.TestAggregate.tier_list_format;

public class TierListTestTier : Entity<TierListTestTierId>
{
    private TierListTestTier() { }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public ushort? MaxItemsCountLimit { get; private set; } //if null => disabled
    public TierListTestTierStyles Styles { get; private set; }

    public static ErrOr<TierListTestTier> CreateNew(string name) {
        if (TierListTestTiersRules.CheckIfStringCorrectTierName(name).IsErr(out var err)) {
            return err;
        }

        return new TierListTestTier() {
            Name = name,
            Description = null,
            MaxItemsCountLimit = null,
            Styles = TierListTestTierStyles.Default()
        };
    }

    public ErrOrNothing Update(
        string newName,
        string? newDescription,
        ushort? newMaxItemsCountLimit,
        TierListTestTierStyles newStyles
    ) {
        if (
            TierListTestTiersRules.CheckIfStringCorrectTierName(newName).IsErr(out var err)
            || TierListTestTiersRules.CheckIfStringCorrectTierDescription(newDescription).IsErr(out err)
            || TierListTestTiersRules.CheckTierItemsCountLimit(newMaxItemsCountLimit).IsErr(out err)
        ) {
            return err;
        }

        Name = newName;
        Description = newDescription;
        MaxItemsCountLimit = newMaxItemsCountLimit;
        Styles.Update(newStyles.BackgroundColor, newStyles.TextColor);
        return ErrOrNothing.Nothing;
    }

    public TierListTestPublishedTierDto ToTestPublishedDto(ushort order) => new(
        Id,
        order,
        Name: Name,
        Description: Description,
        MaxItemsCountLimit: MaxItemsCountLimit,
        new TierListTestPublishedTierStylesDto(
            BackgroundColor: Styles.BackgroundColor,
            TextColor: Styles.TextColor
        )
    );
}