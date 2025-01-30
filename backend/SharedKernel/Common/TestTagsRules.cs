using System.Text.RegularExpressions;

namespace SharedKernel.Common;

public static class TestTagsRules
{
    public const int MaxTagLength = 30;
    public const int MaxTagsForTestCount = 128;

    public static readonly Regex TagRegex =
        new Regex(@"^[a-zA-Zа-яА-Я0-9\+\-_]{1," + MaxTagLength + "}$");
    public static bool IsStringValidTag(string tag) => TagRegex.IsMatch(tag);
}
