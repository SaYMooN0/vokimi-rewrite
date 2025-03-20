using MediatR;
using SharedKernel.IntegrationEvents.test_managing.feedback_left;
using TestManagingService.Application.Common.interfaces.repositories.feedback_records;
using TestManagingService.Domain.FeedbackRecordAggregate.tier_list_test;

namespace TestManagingService.Application.TestFeedbackRecords.integration_events;

public class FeedbackForTierListTestLeftIntegrationEventHandler 
    : INotificationHandler<FeedbackForTierListTestLeftIntegrationEvent>
{
    private readonly ITierListTestFeedbackRecordsRepository _tierListTestFeedbackRecordsRepository;

    public FeedbackForTierListTestLeftIntegrationEventHandler(
        ITierListTestFeedbackRecordsRepository tierListTestFeedbackRecordsRepository
    ) {
        _tierListTestFeedbackRecordsRepository = tierListTestFeedbackRecordsRepository;
    }

    public async Task Handle(
        FeedbackForTierListTestLeftIntegrationEvent notification, CancellationToken cancellationToken
    ) {
        var record = notification.WasLeftAnonymously
            ? TierListTestFeedbackRecord.CreateNewAnonymous(
                notification.TestId, notification.CreatedOn, notification.Text
            )
            : TierListTestFeedbackRecord.CreateNewNonAnonymous(
                notification.TestId, notification.AuthorId, notification.CreatedOn, notification.Text
            );
        await _tierListTestFeedbackRecordsRepository.Add(record);
    }
}