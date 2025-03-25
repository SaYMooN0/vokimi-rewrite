using System.Collections.Immutable;
using MediatR;
using TestTakingService.Application.Common.interfaces.repositories.test_taken_records;
using TestTakingService.Domain.TestTakenRecordAggregate.events;
using TestTakingService.Domain.TestTakenRecordAggregate.tier_list_test;

namespace TestTakingService.Application.TestTakenRecords.events;

public class TierListTestTakenEventHandler : INotificationHandler<TierListTestTakenEvent>
{
    private readonly ITierListTestTakenRecordsRepository _tierListTestTakenRecordsRepository;

    public TierListTestTakenEventHandler(ITierListTestTakenRecordsRepository tierListTestTakenRecordsRepository) {
        _tierListTestTakenRecordsRepository = tierListTestTakenRecordsRepository;
    }

    public async Task Handle(TierListTestTakenEvent notification, CancellationToken cancellationToken) {
        TierListTestTakenRecord record = new(
            notification.TestTakenRecordId,
            notification.AppUserId,
            notification.TestId,
            notification.TestTakingStart,
            notification.TestTakingEnd,
            notification.TestTakenQuestionDetails.Select(kvp => TierListTestTakenRecordTierDetails.CreateNew(
                kvp.Key,
                kvp.Value
            )).ToImmutableArray()
        );
        await _tierListTestTakenRecordsRepository.Add(record);
    }
}