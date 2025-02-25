using ApiShared.interfaces;
using TestCatalogService.Domain.Rules;
using TestCatalogService.Domain.TestAggregate.formats_shared.comment_reports;

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