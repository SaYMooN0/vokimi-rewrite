using SharedKernel.Common.domain;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests.general_format;
using TestCreationService.Domain.Common;

namespace TestCreationService.Domain.TestAggregate.general_format;

public class GeneralTestTakingProcessSettings : Entity<GeneralTestTakingProcessSettingsId>
{
    private GeneralTestTakingProcessSettings() { }
    protected TestId TestId { get; init; }
    public bool ForceSequentialFlow { get; private set; }
    public GeneralTestFeedbackOption Feedback { get; private set; }
    public static GeneralTestTakingProcessSettings CreateNew() => new() {
        Id = GeneralTestTakingProcessSettingsId.CreateNew(),
        ForceSequentialFlow = false,
        Feedback = GeneralTestFeedbackOption.Disabled.Instance
    };
    public void Update(bool forceSequentialFlow, GeneralTestFeedbackOption feedback) {
        ForceSequentialFlow = forceSequentialFlow;
        Feedback = feedback;
    }
}
