using SharedKernel.Common;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using TestCreationService.Domain.Common;

namespace TestCreationService.Domain.TestAggregate.general_format;

public class GeneralTestTakingProcessSettings : Entity
{
    private GeneralTestTakingProcessSettings() { }
    protected override EntityId EntityId => Id;

    public GeneralTestTakingProcessSettingsId Id { get; init; }
    protected TestId TestId { get; init; }
    public bool ForceSequentialFlow { get; private set; }
    public TestFeedbackOption Feedback { get; private set; }
    public static GeneralTestTakingProcessSettings CreateNew() => new() {
        Id = GeneralTestTakingProcessSettingsId.CreateNew(),
        ForceSequentialFlow = false,
        Feedback = TestFeedbackOption.Disabled.Instance
    };
    public void Update(bool forceSequentialFlow, TestFeedbackOption feedback) {
        ForceSequentialFlow = forceSequentialFlow;
        Feedback = feedback;
    }
}
