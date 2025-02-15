using SharedKernel.Common.domain.aggregate_root;
using SharedKernel.Common.domain.entity_id;
using TestCatalogService.Domain.Common;

namespace TestCatalogService.Domain.TestRatingAggregate;

public class TestRating : AggregateRoot<TestRatingId>
{
    public AppUserId Author { get; init; }
    public TestId TestId { get; init; }
}