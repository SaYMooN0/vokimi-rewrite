using ApiShared.interfaces;
using TestCatalogService.Domain.Rules;

namespace TestCatalogService.Api.Contracts.view_test.comments.edit_comment;

public class EditTestCommentRequest : IRequestWithValidationNeeded
{
    public string NewText { get; init; }

    public RequestValidationResult Validate() {
        if (TestCommentRules.CheckCommentTextForErrs(NewText).IsErr(out var err)) {
            return err;
        }

        return RequestValidationResult.Success;
    }
}