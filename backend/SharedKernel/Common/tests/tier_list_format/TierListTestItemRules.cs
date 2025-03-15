using SharedKernel.Common.errors;

namespace SharedKernel.Common.tests.tier_list_format;

public static class TierListTestItemRules
{
    private const int
        MinItemNameLen = 4,
        MaxItemNameLen = 60,
        MaxItemClarificationLen = 120;

    private const int
        NonTextDataMinLen = 8,
        NonTextDataMaxLen = 500;

    private const int
        ItemContentTextMinLen = 2,
        ItemContentTextMaxLen = 96;


    public static ErrOrNothing CheckIdStringCorrectRequiredNonTextItemContent(
        string str, string contentDataType
    ) {
        int len = string.IsNullOrWhiteSpace(str) ? 0 : str.Length;
        if (len < NonTextDataMinLen) {
            return Err.ErrFactory.InvalidData(
                contentDataType + " data is either empty or too short",
                details: $"Content length is {len}. Minimum required length is {NonTextDataMinLen}"
            );
        }

        if (len > NonTextDataMaxLen) {
            return Err.ErrFactory.InvalidData(
                contentDataType + " data is either too long. Please try to somehow shorten it",
                details: $"Content length is {len}. Maximum possible length is {NonTextDataMaxLen}"
            );
        }

        return ErrOrNothing.Nothing;
    }

    public static ErrOrNothing CheckIfStringCorrectAudioTranscriptionItemContent(string? str) {
        if (string.IsNullOrWhiteSpace(str) || str.Length <= NonTextDataMaxLen) {
            return ErrOrNothing.Nothing;
        }

        return Err.ErrFactory.InvalidData(
            $"Audio transcription data must be shorter than {NonTextDataMaxLen} characters",
            details: $"Current length: {str.Length}"
        );
    }

    public static ErrOrNothing CheckIfStringCorrectItemTextContent(string? str) {
        int len = string.IsNullOrWhiteSpace(str) ? 0 : str.Length;
        if (len < ItemContentTextMinLen || len > ItemContentTextMaxLen) {
            return Err.ErrFactory.InvalidData(
                $"Item text must be between {ItemContentTextMinLen} and {ItemContentTextMaxLen} characters",
                details: $"Current length: {len}"
            );
        }

        return ErrOrNothing.Nothing;
    }

    public static ErrOrNothing CheckIfStringCorrectItemName(string name) {
        int len = string.IsNullOrWhiteSpace(name) ? 0 : name.Length;
        if (len < MinItemNameLen) {
            return Err.ErrFactory.InvalidData(
                $"Item name is too short. Minimum allowed length is {MinItemNameLen} characters",
                details: $"Current length: {len}"
            );
        }

        if (len > MaxItemNameLen) {
            return Err.ErrFactory.InvalidData(
                $"Item name is too long. Maximum allowed length is {MaxItemNameLen} characters",
                details: $"Current length: {len}"
            );
        }

        return ErrOrNothing.Nothing;
    }

    public static ErrOrNothing CheckIfStringCorrectItemClarification(string? clarification) {
        int len = string.IsNullOrWhiteSpace(clarification) ? 0 : clarification.Length;
        if (len > MaxItemClarificationLen) {
            return Err.ErrFactory.InvalidData(
                $"Item clarification is too long. Maximum allowed length is {MaxItemClarificationLen} characters",
                details: $"Current length: {len}"
            );
        }

        return ErrOrNothing.Nothing;
    }
}