using SharedKernel.Common;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.interfaces;

namespace TestTakingService.Domain.UnitTests.TestAggregateRoot;

public static class TestSharedTestsConsts
{
    public static readonly AppUserId TestTakerId = new(Guid.NewGuid());
    public static DateTime TestTakingStart => DateTime.Now.AddHours(-1);
    public static DateTime TestTakingEnd => DateTime.Now;

    public static IDateTimeProvider DateTimeProviderInstance = new UtcDateTimeProvider();
}