using ApiShared.interfaces;
using SharedKernel.Common.common_enums;
using SharedKernel.Common.errors;
using TestCreationService.Domain.Common.rules;
using TestCreationService.Domain.TestAggregate.general_format;

namespace TestCreationService.Api.Contracts.Tests.test_creation.general_format;

public class UpdateGeneralTestTakingProcessSettingsRequest : IRequestWithValidationNeeded
{
    public bool ForceSequentialFlow { get; init; }
    public bool EnableTestFeedback { get; init; }
    public AnonymityValues FeedbackAnonymity { get; init; }
    public string AccompanyingText { get; init; }
    public ushort MaxFeedbackLength { get; init; }

    public RequestValidationResult Validate() {
        if (CreateFeedbackOption().IsErr(out var feedbackCreationErr)) {
            return feedbackCreationErr;
        }
        return RequestValidationResult.Success;
    }
    public ErrOr<GeneralTestFeedbackOption> CreateFeedbackOption() {
        if (!EnableTestFeedback) {
            return GeneralTestFeedbackOption.Disabled.Instance;
        }
        var enabledCreatingRes = GeneralTestFeedbackOption.Enabled.CreateNew(
            FeedbackAnonymity,
            AccompanyingText,
            MaxFeedbackLength
        );
        if (enabledCreatingRes.IsErr(out var err)) {
            return err;
        }
        return enabledCreatingRes.GetSuccess();
    }
}
