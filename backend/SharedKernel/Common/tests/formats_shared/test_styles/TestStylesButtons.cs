

using SharedKernel.Common.domain;

namespace SharedKernel.Common.tests.formats_shared.test_styles;

public class TestStylesButtons : ValueObject
{
    private TestStylesButtons() { }
    public TestStylesButtonContent Content { get; init; }
    public TestStylesButtonFillType FillType { get; init; }
    public ushort IconsKey { get; init; }
    public static TestStylesButtons Default => new() {
        Content = TestStylesButtonContent.TextWithIconVar1,
        FillType = TestStylesButtonFillType.Solid,
        IconsKey = 1
    };
    public TestStylesButtons(TestStylesButtonContent content, TestStylesButtonFillType fillType, ushort iconKey) {
        Content = content;
        FillType = fillType;
        IconsKey = iconKey;
    }
    public override IEnumerable<object> GetEqualityComponents() {
        yield return Content;
        yield return FillType;
        yield return IconsKey;
    }
}
