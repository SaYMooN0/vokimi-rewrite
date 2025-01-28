using ApiShared.interfaces;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests.test_styles;
using SharedKernel.Common.tests.value_objects;

namespace TestCreationService.Api.Contracts.Tests.test_creation.formats_shared;

public record class UpdateTestStylesRequest(
    string AccentColor,
    string ErrorsColor,
    TestStylesButtonContent ButtonsContent,
    TestStylesButtonFillType ButtonsFillType,
    int ButtonIconsKey
) : IRequestWithValidationNeeded
{
    public RequestValidationResult Validate() {
        ErrList errs = new();
        errs.AddPossibleErr(ParsedAccentColor());
        errs.AddPossibleErr(ParsedErrorsColor());
        errs.AddPossibleErr(ParsedTestStylesButtons());
        return errs;
    }
    public ErrOr<HexColor> ParsedAccentColor() => HexColor.FromString(AccentColor);
    public ErrOr<HexColor> ParsedErrorsColor() => HexColor.FromString(ErrorsColor);
    public ErrOr<TestStylesButtons> ParsedTestStylesButtons() {
        if (ButtonIconsKey < 0 || ButtonIconsKey > ushort.MaxValue) {
            return Err.ErrFactory.InvalidData("Incorrect button icons", details: "Incorrect button icons key format");
        }
        return new TestStylesButtons(ButtonsContent, ButtonsFillType, (ushort)ButtonIconsKey);
    }
}
