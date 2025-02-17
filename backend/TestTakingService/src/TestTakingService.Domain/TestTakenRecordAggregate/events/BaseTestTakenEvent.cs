using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;
using TestTakingService.Domain.Common;

namespace TestTakingService.Domain.TestTakenRecordAggregate.events;

public abstract record class BaseTestTakenEvent(
    TestTakenRecordId TestTakenRecordId,
    TestId TestId,
    AppUserId? AppUserId,
    DateTime TestTakingStart,
    DateTime TestTakingEnd
) : IDomainEvent;