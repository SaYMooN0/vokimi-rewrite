using AuthenticationService.Domain.Common.value_objects;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.interfaces;

namespace DBSeeder.Data.users.shared;

public class AppUserInstance
{
    public AuthenticationService.Domain.AppUserAggregate.AppUser AuthAppUser { get; }
    public TestCatalogService.Domain.AppUserAggregate.AppUser TestCatalogAppUser { get; }
    public TestCreationService.Domain.AppUserAggregate.AppUser TestCreationAppUser { get; }
    public TestManagingService.Domain.AppUserAggregate.AppUser TestManagingAppUser { get; }

    private AppUserInstance(
        AuthenticationService.Domain.AppUserAggregate.AppUser authAppUser,
        TestCatalogService.Domain.AppUserAggregate.AppUser testCatalogAppUser,
        TestCreationService.Domain.AppUserAggregate.AppUser testCreationAppUser,
        TestManagingService.Domain.AppUserAggregate.AppUser testManagingAppUser
    ) {
        AuthAppUser = authAppUser;
        TestCatalogAppUser = testCatalogAppUser;
        TestCreationAppUser = testCreationAppUser;
        TestManagingAppUser = testManagingAppUser;
    }

    public static AppUserInstance CreateNew(IDateTimeProvider dateTimeProvider) {
        var email = Email.Create($"user{Guid.NewGuid()}@example.com").GetSuccess();
        var password = $"random-password-{Guid.NewGuid()}";
        return CreateSpecified(email, password, DateTime.Now);
    }

    public static AppUserInstance CreateSpecified(
        Email email,
        string password,
        DateTime registrationDate,
        HashSet<TestId>? publishedCreatedTests = null,
        HashSet<TestId>? publishedEditorAssignedTests = null,
        HashSet<TestId>? draftCreatedTests = null,
        HashSet<TestId>? draftEditorAssignedTests =  null
    ) {
        string passwordHash = DataShared.PasswordHasher.HashPassword(password);
        var authAppUser = AuthenticationService.Domain.AppUserAggregate.AppUser.CreateNew(
            email, passwordHash, DataShared.DateTimeProviderWithNowSet(registrationDate)
        );

        var testCatalogAppUser = new TestCatalogService.Domain.AppUserAggregate.AppUser(authAppUser.Id);
        var testCreationAppUser = new TestCreationService.Domain.AppUserAggregate.AppUser(authAppUser.Id);
        var testManagingAppUser = new TestManagingService.Domain.AppUserAggregate.AppUser(authAppUser.Id);

        if (publishedCreatedTests is not null) {
            foreach (var id in publishedCreatedTests) {
                testCatalogAppUser.AddCreatedTest(id);
                testManagingAppUser.AddCreatedTest(id);
            }
        }

        if (publishedEditorAssignedTests is not null) {
            foreach (var id in publishedEditorAssignedTests) {
                testCatalogAppUser.AddEditorRoleForTest(id);
                testManagingAppUser.AddEditorRoleForTest(id);
            }
        }

        if (draftCreatedTests is not null) {
            foreach (var id in draftCreatedTests) {
                testCreationAppUser.AddCreatedTest(id);
            }
        }

        if (draftEditorAssignedTests is not null) {
            foreach (var id in draftEditorAssignedTests) {
                testCreationAppUser.AddEditorRoleForTest(id);
            }
        }

        return new AppUserInstance(authAppUser, testCatalogAppUser, testCreationAppUser, testManagingAppUser);
    }
}