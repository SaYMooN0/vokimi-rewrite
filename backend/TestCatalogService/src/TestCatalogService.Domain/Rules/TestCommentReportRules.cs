using SharedKernel.Common.errors;

namespace TestCatalogService.Domain.Rules;

public static class TestCommentReportRules
{
    public const int
        MinReportTextLength = 10,
        MaxReportTextLength = 250;

    public static ErrOrNothing CheckReportTextForErr(string text) {
        int len = string.IsNullOrWhiteSpace(text) ? 0 : text.Length;

        if (len < MinReportTextLength) {
            return Err.ErrFactory.InvalidData(
                $"The report text is too short. It must be at least {MinReportTextLength} characters long",
                $"Current length is {len}"
            );
        }

        if (len > MaxReportTextLength) {
            return Err.ErrFactory.InvalidData(
                $"The report text is too long. It must be no more than {MaxReportTextLength} characters long",
                $"Current length is {len}"
            );
        }

        return ErrOrNothing.Nothing;
    }
}