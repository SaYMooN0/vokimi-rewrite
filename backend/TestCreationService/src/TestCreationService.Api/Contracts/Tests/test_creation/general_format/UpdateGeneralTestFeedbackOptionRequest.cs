using ApiShared.interfaces;
using SharedKernel.Common.common_enums;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests.general_format;

namespace TestCreationService.Api.Contracts.Tests.test_creation.general_format;

public record class UpdateGeneralTestFeedbackOptionRequest(
    bool EnableTestFeedback,
    AnonymityValues FeedbackAnonymity,
    string AccompanyingText,
    ushort MaxFeedbackLength
) : IRequestWithValidationNeeded
{
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
