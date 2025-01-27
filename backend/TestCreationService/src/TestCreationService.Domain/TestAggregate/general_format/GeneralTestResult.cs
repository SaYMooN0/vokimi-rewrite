using SharedKernel.Common;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using System.Collections.Immutable;
using TestCreationService.Domain.Rules;

namespace TestCreationService.Domain.TestAggregate.general_format;

public class GeneralTestResult : Entity<GeneralTestResultId>
{
    private GeneralTestResult() { }
    public TestId TestId { get; init; }
    public string Name { get; private set; }
    public string Text { get; private set; }
    public string? Image { get; private set; }
    public static ErrOr<GeneralTestResult> CreateNew(TestId testId, string name) {
        if (GeneralFormatTestRules.CheckResultNameForErrs(name).IsErr(out var err)) {
            return err;
        }
        return new GeneralTestResult() {
            Id = GeneralTestResultId.CreateNew(),
            TestId = testId,
            Name = name,
            Text = "General test result text",
            Image = null
        };
    }
}
