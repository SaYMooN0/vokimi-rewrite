using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;
using TestTakingService.Domain.Common;

namespace TestTakingService.Domain.AppUserAggregate.events;

public record class TestTakenByAuthenticatedUserEvent(
    AppUserId AppUserId,
    TestId TestId,
    TestTakenRecordId TestTakenRecordId
) : IDomainEvent;