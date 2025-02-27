using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.interfaces;
using TestCatalogService.Domain.Common;

namespace TestCatalogService.Domain.TestAggregate.formats_shared.ratings;

public class TestRating : Entity<TestRatingId>
{
    private TestRating() { }
    public ushort Value { get; private set; }
    public AppUserId UserId { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? LastUpdated { get; protected set; }
    public const ushort MaxValue = 5;

    public static ErrOr<TestRating> CreateNew(
        ushort value,
        AppUserId userId,
        IDateTimeProvider dateTimeProvider
    ) {
        if (value > MaxValue) {
            return Err.ErrFactory.InvalidData($"Test rating cannot be greater than {MaxValue}");
        }

        TestRating rating = new() {
            Id = TestRatingId.CreateNew(),
            Value = value,
            UserId = userId,
            CreatedAt = dateTimeProvider.Now,
            LastUpdated = null
        };
        return rating;
    }

    public ErrOrNothing Update(ushort newValue, IDateTimeProvider dateTimeProvider) {
        if (newValue > MaxValue) {
            return new Err($"Test rating cannot be greater than {MaxValue}");
        }

        Value = newValue;
        LastUpdated = dateTimeProvider.Now;
        return ErrOrNothing.Nothing;
    }
}