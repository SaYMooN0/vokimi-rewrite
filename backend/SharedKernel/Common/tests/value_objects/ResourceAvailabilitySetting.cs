using SharedKernel.Common.common_enums;

namespace SharedKernel.Common.tests.value_objects;

public class ResourceAvailabilitySetting : ValueObject
{
    public bool IsEnabled { get; init; }
    public AccessLevel Access { get; init; }

    public ResourceAvailabilitySetting(bool isEnabled, AccessLevel access) {
        IsEnabled = isEnabled;
        Access = access;
    }

    public static ResourceAvailabilitySetting Disabled => new(false, AccessLevel.Private);

    public static ResourceAvailabilitySetting EnabledPrivate => new(true, AccessLevel.Private);

    public static ResourceAvailabilitySetting EnabledFollowersOnly => new(true, AccessLevel.FollowersOnly);

    public static ResourceAvailabilitySetting EnabledPublic => new(true, AccessLevel.Public);

    public override IEnumerable<object> GetEqualityComponents() => IsEnabled ? [IsEnabled, Access] : [IsEnabled];
}
