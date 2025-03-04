using System.Collections.Immutable;
using AuthenticationService.Domain.Common.value_objects;
using SharedKernel.Common.domain.entity;

namespace DBSeeder.Data.users;

internal static class AppUsersData
{
    public static readonly AppUserInstance User1 = AppUserInstance.CreateSpecified(
        Email.Create("admin@admin.com").GetSuccess(),
        "admin@admin.com",
        DateTime.Now.AddDays(-10).AddMonths(-13),
        publishedCreatedTests: [TestId.CreateNew()]
    );

    public static readonly AppUserInstance User2 = AppUserInstance.CreateSpecified(
        Email.Create("user2@admin.com").GetSuccess(),
        "user2@admin.com",
        DateTime.Now.AddDays(-110).AddMonths(-1)
    );

    public static readonly AppUserInstance User3 = AppUserInstance.CreateSpecified(
        Email.Create("user3@admin.com").GetSuccess(),
        "user3@admin.com",
        DateTime.Now.AddDays(12).AddMonths(-9)
    );

    public static readonly ImmutableArray<AppUserInstance> AllUsers = [
        User1, User2, User3
    ];
}