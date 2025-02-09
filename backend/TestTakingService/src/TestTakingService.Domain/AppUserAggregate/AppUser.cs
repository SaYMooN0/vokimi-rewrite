using System.Collections.Immutable;
using SharedKernel.Common.domain;

namespace TestTakingService.Domain.AppUserAggregate;

public class AppUser : AggregateRoot<AppUserId>
{
    private readonly HashSet<TestId> _takenTestIds;
    private AppUser() { }

    public AppUser(AppUserId id) {
        Id = id;
        _takenTestIds = [];
    }

    public ImmutableHashSet<TestId> TakenTestIds => _takenTestIds.ToImmutableHashSet();
}