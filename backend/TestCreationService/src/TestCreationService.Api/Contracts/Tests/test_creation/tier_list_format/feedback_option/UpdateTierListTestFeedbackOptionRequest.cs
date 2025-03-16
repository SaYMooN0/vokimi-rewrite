using ApiShared.interfaces;
using SharedKernel.Common.common_enums;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests.tier_list_format.feedback;

namespace TestCreationService.Api.Contracts.Tests.test_creation.tier_list_format.feedback_option;

public class UpdateTierListTestFeedbackOptionRequest : IRequestWithValidationNeeded
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

    public ErrOr<TierListTestFeedbackOption> CreateFeedbackOption() {
        if (!EnableTestFeedback) {
            return TierListTestFeedbackOption.Disabled.Instance;
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

        var enabledCreatingRes = TierListTestFeedbackOption.Enabled.CreateNew(
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