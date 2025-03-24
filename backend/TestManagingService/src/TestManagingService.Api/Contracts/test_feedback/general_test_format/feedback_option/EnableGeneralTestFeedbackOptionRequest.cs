using ApiShared.interfaces;
using SharedKernel.Common.common_enums;
using SharedKernel.Common.tests.general_format;

namespace TestManagingService.Api.Contracts.test_feedback.general_test_format.feedback_option;

public class EnableGeneralTestFeedbackOptionRequest : IRequestWithValidationNeeded
{
    public AnonymityValues Anonymity { get; init; }
    public string AccompanyingText { get; init; }
    public ushort MaxFeedbackLength { get; init; }

    public RequestValidationResult Validate() {
        if (GeneralTestFeedbackRules.CheckAccompanyingTextForErrs(AccompanyingText).IsErr(out var err)) {
            return err;
        }
        if (GeneralTestFeedbackRules.CheckMaxFeedbackLengthForErrs(MaxFeedbackLength).IsErr(out err)) {
            return err;
        }
        return RequestValidationResult.Success;
    }

}