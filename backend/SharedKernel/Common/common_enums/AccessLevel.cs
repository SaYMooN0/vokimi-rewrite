namespace SharedKernel.Common.common_enums;

public enum AccessLevel
{
    Private = 0,
    FollowersOnly = 1,
    Public = 2
}
public static class AccessLevelExtensions
{
    public static bool IsStricterThan(this AccessLevel first, AccessLevel second) {
        return first < second;
    }
}