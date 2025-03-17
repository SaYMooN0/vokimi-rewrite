using ApiShared.interfaces;
using SharedKernel.Common.common_enums;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests.general_format;

namespace TestCreationService.Api.Contracts.Tests.test_creation.general_format.feedback_option;

internal class UpdateGeneralTestFeedbackOptionRequest : IRequestWithValidationNeeded
{
    public bool EnableTestFeedback { get; init; } = false;
    public AnonymityValues? FeedbackAnonymity { get; init; } = null;
    public string? AccompanyingText { get; init; } = null;
    public ushort? MaxFeedbackLength { get; init; } = null;

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

        if (FeedbackAnonymity is null) {
            return Err.ErrFactory.InvalidData("For enabled feedback anonymity value is required");
        }

        if (AccompanyingText is null) {
            return Err.ErrFactory.InvalidData("For enabled feedback accompanying text value is required");
        }

        if (MaxFeedbackLength is null) {
            return Err.ErrFactory.InvalidData("For enabled feedback max feedback length value is required");
        }

        var enabledCreatingRes = GeneralTestFeedbackOption.Enabled.CreateNew(
            FeedbackAnonymity.Value,
            AccompanyingText,
            MaxFeedbackLength.Value
        );
        if (enabledCreatingRes.IsErr(out var err)) {
            return err;
        }

        return enabledCreatingRes.GetSuccess();
    }
}