using SharedKernel.Common.domain.entity;
using SharedKernel.Common.tests.formats_shared.test_styles;
using SharedKernel.Common.tests.value_objects;

namespace SharedKernel.Common.tests.tier_list_format;

public class TierListTestTierStyles : Entity<TierListTestTierStylesId>
{
    private TierListTestTierStyles() { }
    public HexColor BackgroundColor { get; private set; }
    public HexColor TextColor { get; private set; }

    public static TierListTestTierStyles CreateNew(HexColor backgroundColor, HexColor textColor) => new() {
        Id = TierListTestTierStylesId.CreateNew(),
        BackgroundColor = backgroundColor,
        TextColor = textColor
    };

    public static TierListTestTierStyles Default() => TierListTestTierStyles.CreateNew(
        backgroundColor: TestStylesSheet.DefaultAccentColor,
        textColor: HexColor.FromString("#ffffff").GetSuccess()
    );

    public void Update(HexColor backgroundColor, HexColor textColor) {
        BackgroundColor = backgroundColor;
        TextColor = textColor;
    }
}