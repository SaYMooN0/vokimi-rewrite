using SharedKernel.Common.errors;

namespace TestCatalogService.Domain.Rules;

public static class TestCommentRules
{
    public const int MaxCommentLength = 2000;

    public static ErrOrNothing CheckCommentTextForErrs(string text) {
        if (text is null) {
            return new Err("Comment text is unset");
        }

        int len = text.Length;
        if (len > MaxCommentLength) {
            return Err.ErrFactory.InvalidData(
                $"Comment text is too long. Maximum length is {MaxCommentLength}. Current length is {len}"
            );
        }

        return ErrOrNothing.Nothing;
    }
}