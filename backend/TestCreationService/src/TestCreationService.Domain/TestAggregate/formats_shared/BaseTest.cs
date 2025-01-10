
using SharedKernel.Common.EntityIds;

namespace TestCreationService.Domain.TestAggregate.formats_shared;

public abstract class BaseTest
{
    public TestId Id { get; init; }
    public AppUserId Creator { get; init; }
    public List<AppUserId> Editors { get; init; }
}
