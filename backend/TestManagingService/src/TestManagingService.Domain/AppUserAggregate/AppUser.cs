using SharedKernel.Common.domain.aggregate_root;
using SharedKernel.Common.domain.entity;

namespace TestManagingService.Domain.AppUserAggregate;

public class AppUser : AggregateRoot<AppUserId>
{
    private AppUser() { }
    private readonly HashSet<TestId> _createdTestIds;
    private readonly HashSet<TestId> _editorAssignedTests;
    public AppUser(AppUserId id) {
        Id = id;
        _createdTestIds = [];
        _editorAssignedTests = [];
    }

    public void AddCreatedTest(TestId testId) =>
        _createdTestIds.Add(testId);

    public void AddEditorRoleForTest(TestId testId) =>
        _editorAssignedTests.Add(testId);
}