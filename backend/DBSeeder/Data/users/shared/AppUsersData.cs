using System.Collections.Immutable;
using AuthenticationService.Domain.Common.value_objects;

namespace DBSeeder.Data.users.shared;

internal static class AppUsersData
{
    public static readonly AppUserInstance User1 = AppUserInstance.CreateSpecified(
        Email.Create("admin@admin.com").GetSuccess(),
        "admin@admin.com",
        DateTime.Now.AddDays(-10).AddMonths(-13)
    );
    public static readonly ImmutableArray<AppUserInstance> AllUsers = [
        User1
    ];

    
}