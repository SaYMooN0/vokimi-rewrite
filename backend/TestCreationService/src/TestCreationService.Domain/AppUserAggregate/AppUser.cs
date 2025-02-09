using SharedKernel.Common.domain;
using System.Collections.Immutable;

namespace TestCreationService.Domain.AppUserAggregate;

public class AppUser : AggregateRoot<AppUserId>
{
    private AppUser() { }
    private readonly HashSet<TestId> _createdTestIds;
    public ImmutableHashSet<TestId> CreatedTestIds => _createdTestIds.ToImmutableHashSet();

    private readonly HashSet<TestId> _editorAssignedTests;
    public ImmutableHashSet<TestId> EditorAssignedTests => _editorAssignedTests.ToImmutableHashSet();

    public void AddCreatedTest(TestId testId) {
        _createdTestIds.Add(testId);
    }

    public void RemoveCreatedTest(TestId testId) {
        _createdTestIds.Remove(testId);
    }

    public void AddEditorRoleForTest(TestId testId) {
        _editorAssignedTests.Add(testId);
    }

    public void RemoveEditorRoleForTest(TestId testId) {
        _editorAssignedTests.Remove(testId);
    }

    public AppUser(AppUserId id) {
        Id = id;
        _createdTestIds = [];
        _editorAssignedTests = [];
    }
}