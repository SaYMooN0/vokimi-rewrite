using SharedKernel.Common;
using SharedKernel.Common.EntityIds;
using System.Collections.Immutable;
using TestCreationService.Domain.TestAggregate;

namespace TestCreationService.Domain.AppUserAggregate;

public class AppUser : AggregateRoot
{
    protected override EntityId EntityId => Id;

    public AppUserId Id { get; init; }
    private readonly HashSet<TestId> _createdTestIds = [];
    public ImmutableHashSet<TestId> CreatedTestIds => _createdTestIds.ToImmutableHashSet();
    private readonly HashSet<TestId> _editorAssignedTests = new();
    public ImmutableHashSet<TestId> EditorAssignedTests => _editorAssignedTests.ToImmutableHashSet();
    public void AddCreatedTest(TestId testId) {
        if (!_createdTestIds.Contains(testId)) {
            _createdTestIds.Add(testId);
        }
    }
}
