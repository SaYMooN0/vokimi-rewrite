using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using TestCreationService.Domain.Rules;

namespace TestCreationService.Domain.TestAggregate.general_format;

public class GeneralTestResult : Entity<GeneralTestResultId>
{
    private GeneralTestResult() { }
    public TestId TestId { get; init; }
    public string Name { get; private set; }
    public string Text { get; private set; }
    public string Image { get; private set; }
    public static ErrOr<GeneralTestResult> CreateNew(TestId testId, string name) {
        if (GeneralFormatTestRules.CheckResultNameForErrs(name).IsErr(out var err)) {
            return err;
        }
        return new GeneralTestResult() {
            Id = GeneralTestResultId.CreateNew(),
            TestId = testId,
            Name = name,
            Text = "General test result text",
            Image = string.Empty
        };
    }
    public ErrOrNothing Update(string name, string text, string image) {
        if (GeneralFormatTestRules.CheckResultNameForErrs(name).IsErr(out var err)) {
            return err;
        }
        if (GeneralFormatTestRules.CheckResultTextForErrs(text).IsErr(out err)) {
            return err;
        }
        int imgLen = string.IsNullOrEmpty(image) ? 0 : image.Length;
        if (imgLen > 1000) {
            return Err.ErrFactory.InvalidData("Image path is too long", details: "Try to somehow shorten it ");
        }
        Name = name;
        Text = text;
        Image = image;
        return ErrOrNothing.Nothing;
    }
}
