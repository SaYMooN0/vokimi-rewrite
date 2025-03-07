using ApiShared.interfaces;
using SharedKernel.Common.common_enums;
using SharedKernel.Common.tests.general_format;

namespace TestManagingService.Api.Contracts.test_feedback;

public class EnableGeneralTestFeedbackRequest : IRequestWithValidationNeeded
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