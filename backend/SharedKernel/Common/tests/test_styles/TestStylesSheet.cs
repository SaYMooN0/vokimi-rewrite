using SharedKernel.Common.domain;
using SharedKernel.Common.tests.value_objects;

namespace SharedKernel.Common.tests.test_styles;

public class TestStylesSheet : Entity<TestStylesSheetId>
{
    private TestStylesSheet() { }
    private TestId TestId { get; init; }
    public HexColor AccentColor { get; private set; }
    public HexColor ErrorsColor { get; private set; }
    public TestStylesButtons Buttons { get; private set; }

    private static readonly HexColor _defaultAccentColor = HexColor.FromString("#796cfa").GetSuccess();
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
        _defaultAccentColor,
        _defaultErrorsColor,
        TestStylesButtons.Default
    );

    public void Update(HexColor accentColor, HexColor errorsColor, TestStylesButtons buttonsStyle) {
        AccentColor = accentColor;
        ErrorsColor = errorsColor;
        Buttons = buttonsStyle;
    }

    public void SetToDefault() {
        AccentColor = _defaultAccentColor;
        ErrorsColor = _defaultErrorsColor;
        Buttons = TestStylesButtons.Default;
    }

    // how test looks in the catalog
    //---View test page
    // how test cover looks like
    // how underlying buttons buttons look like
    //---Test taking page
    //
    //---View results page
}