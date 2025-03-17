using SharedKernel.Common.errors;
using SharedKernel.Common.tests.tier_list_format;
using SharedKernel.Common.tests.value_objects;

namespace TestCreationService.Api.Contracts.Tests.test_creation.tier_list_format.tiers;

internal class TierListTestTierStylesContract
{
    public string BackgroundColor { get; init; } = string.Empty;
    public string TextColor { get; init; } = string.Empty;

    public ErrOr<TierListTestTierStyles> ParseToTierStyles() {
        var backColor = HexColor.FromString(BackgroundColor);
        if (backColor.IsErr()) {
            return Err.ErrFactory.InvalidData("Styles background color is not a valid hex color");
        }

        var textColor = HexColor.FromString(TextColor);
        if (textColor.IsErr()) {
            return Err.ErrFactory.InvalidData("Styles text color is not a valid hex color");
        }

        return new TierListTestTierStyles(
            backgroundColor: backColor.GetSuccess(),
            textColor: textColor.GetSuccess()
        );
    }

    public static TierListTestTierStylesContract FromStyles(TierListTestTierStyles tierStyles) => new() {
        BackgroundColor = tierStyles.BackgroundColor.ToString(),
        TextColor = tierStyles.TextColor.ToString(),
    };
}