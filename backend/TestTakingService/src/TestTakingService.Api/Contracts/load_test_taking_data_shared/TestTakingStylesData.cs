using SharedKernel.Common.tests.test_styles;

namespace TestTakingService.Api.Contracts.load_test_taking_data_shared;

public record class TestTakingStylesData(
    string AccentColor,
    string ErrorsColor,
    TestStylesButtonContent ButtonsContent,
    TestStylesButtonFillType ButtonsFillType,
    ushort ButtonsIconsKey
)
{
    public static TestTakingStylesData FromStyles(TestStylesSheet styles) => new(
        styles.AccentColor.ToString(),
        styles.ErrorsColor.ToString(),
        styles.Buttons.Content,
        styles.Buttons.FillType,
        styles.Buttons.IconsKey
    );
}