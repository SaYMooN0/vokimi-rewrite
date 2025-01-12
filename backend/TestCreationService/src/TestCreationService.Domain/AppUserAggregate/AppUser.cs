using SharedKernel.Common;
using SharedKernel.Common.EntityIds;

namespace TestCreationService.Domain.AppUserAggregate;

public class AppUser : AggregateRoot
{
    protected override EntityId EntityId => Id;

    public AppUserId Id { get; init; }

}
