using ApiShared.interfaces;
using SharedKernel.Common.common_enums;
using TestCatalogService.Domain.Rules;
namespace TestCatalogService.Api.Contracts.view_test.comments;

public class ReportTestCommentRequest : IRequestWithValidationNeeded
{
    public CommentReportReason Reason { get; init; } = CommentReportReason.Custom;
    public string Text { get; init; } = null;

    public RequestValidationResult Validate() {
        if (TestCommentReportRules.CheckReportTextForErr(Text).IsErr(out var err)) {
            return err;
        }
        return RequestValidationResult.Success;
    }
}