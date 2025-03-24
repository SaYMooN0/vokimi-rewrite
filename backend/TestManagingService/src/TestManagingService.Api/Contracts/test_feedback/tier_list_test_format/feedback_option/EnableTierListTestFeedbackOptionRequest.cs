using ApiShared.interfaces;
using SharedKernel.Common.common_enums;
using SharedKernel.Common.tests.tier_list_format.feedback;

namespace TestManagingService.Api.Contracts.test_feedback.tier_list_test_format.feedback_option;

public class EnableTierListTestFeedbackOptionRequest : IRequestWithValidationNeeded
{
    public AnonymityValues Anonymity { get; init; }
    public string AccompanyingText { get; init; }
    public ushort MaxFeedbackLength { get; init; }

    public RequestValidationResult Validate() {
        if (TierListTestFeedbackRules.CheckAccompanyingTextForErrs(AccompanyingText).IsErr(out var err)) {
            return err;
        }
        if (TierListTestFeedbackRules.CheckMaxFeedbackLengthForErrs(MaxFeedbackLength).IsErr(out err)) {
            return err;
        }
        return RequestValidationResult.Success;
    }

}