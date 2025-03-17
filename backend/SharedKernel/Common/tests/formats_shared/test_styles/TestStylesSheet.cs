using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.tests.value_objects;

namespace SharedKernel.Common.tests.formats_shared.test_styles;

public class TestStylesSheet : Entity<TestStylesSheetId>
{
    private TestStylesSheet() { }
    private TestId TestId { get; init; }
    public HexColor AccentColor { get; private set; }
    public HexColor ErrorsColor { get; private set; }
    public TestStylesButtons Buttons { get; private set; }

    public static readonly HexColor DefaultAccentColor = HexColor.FromString("#796cfa").GetSuccess();
    private static readonly HexColor _defaultErrorsColor = HexColor.FromString("#e02926").GetSuccess();

    public TestStylesSheet(
        TestStylesSheetId id,
        TestId testId,
        HexColor accentColor,
        HexColor errorsColor,
        TestStylesButtons buttons
    ) {
        Id = id;
        TestId = testId;
        AccentColor = accentColor;
        ErrorsColor = errorsColor;
        Buttons = buttons;
    }

    public static TestStylesSheet CreateNew(TestId testId) => new(
        TestStylesSheetId.CreateNew(),
        testId,
        DefaultAccentColor,
        _defaultErrorsColor,
        TestStylesButtons.Default
    );

    public void Update(HexColor accentColor, HexColor errorsColor, TestStylesButtons buttonsStyle) {
        AccentColor = accentColor;
        ErrorsColor = errorsColor;
        Buttons = buttonsStyle;
    }

    public void SetToDefault() {
        AccentColor = DefaultAccentColor;
        ErrorsColor = _defaultErrorsColor;
        Buttons = TestStylesButtons.Default;
    }
}