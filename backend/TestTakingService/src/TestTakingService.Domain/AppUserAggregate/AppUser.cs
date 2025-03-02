using System.Collections.Immutable;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.aggregate_root;
using SharedKernel.Common.domain.entity;
using TestTakingService.Domain.Common;

namespace TestTakingService.Domain.AppUserAggregate;

public class AppUser : AggregateRoot<AppUserId>
{
    private readonly HashSet<TestId> _takenTestIds;
    private readonly HashSet<TestTakenRecordId> _testTakenRecordIds;
    private AppUser() { }

    public AppUser(AppUserId id) {
        Id = id;
        _takenTestIds = [];
        _testTakenRecordIds = [];
    }

    public ImmutableHashSet<TestId> TakenTestIds => _takenTestIds.ToImmutableHashSet();
    public ImmutableHashSet<TestTakenRecordId> TestTakenRecordIds => _testTakenRecordIds.ToImmutableHashSet();

    public void AddTakenTestId(TestId testId) {
        _takenTestIds.Add(testId);
    }

    public void AddTestTakenRecordId(TestTakenRecordId testTakenRecordId) {
        _testTakenRecordIds.Add(testTakenRecordId);
    }
}