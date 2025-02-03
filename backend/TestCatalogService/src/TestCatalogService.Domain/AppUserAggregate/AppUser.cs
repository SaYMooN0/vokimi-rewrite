using SharedKernel.Common.domain;
using System.Collections.Immutable;

namespace TestCatalogService.Domain.AppUserAggregate;

public class AppUser : AggregateRoot<AppUserId>
{
    private AppUser() { }
    private readonly HashSet<TestId> _createdTestIds = [];
    public ImmutableHashSet<TestId> CreatedTestIds => _createdTestIds.ToImmutableHashSet();

    private readonly HashSet<TestId> _editorAssignedTests = [];
    public ImmutableHashSet<TestId> EditorAssignedTests => _editorAssignedTests.ToImmutableHashSet();
}
