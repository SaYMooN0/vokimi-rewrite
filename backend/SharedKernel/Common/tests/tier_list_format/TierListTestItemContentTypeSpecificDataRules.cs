namespace SharedKernel.Common.tests.tier_list_format;

public class TierListTestItemContentTypeSpecificDataRules
{
    public const int
        NonTextDataMaxLength = 500;
    public const int
        ItemTextMinLength = 2,
        ItemTextMaxLength = 96;

    public static bool IsStringCorrectNonTextItem(string str, out int length) {
        length = string.IsNullOrWhiteSpace(str) ? 0 : str.Length;
        if (length <= 0 || length > NonTextDataMaxLength) {
            return false;
        }

        return true;
    }

    public static bool IsStringCorrectAudioTranscriptionItemContent(string? str, out int length) {
        length = string.IsNullOrWhiteSpace(str) ? 0 : str.Length;
        return length <= NonTextDataMaxLength;
    } 
    public static bool IsStringCorrectItemText(string? str, out int length) {
        length = string.IsNullOrWhiteSpace(str) ? 0 : str.Length;
        if (length < ItemTextMinLength || length > ItemTextMaxLength) {
            return false;
        }
        return true;
    }
    
}

