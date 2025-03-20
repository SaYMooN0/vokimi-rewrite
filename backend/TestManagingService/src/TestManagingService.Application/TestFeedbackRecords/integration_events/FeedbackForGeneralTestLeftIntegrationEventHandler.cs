using MediatR;
using SharedKernel.IntegrationEvents.test_managing.feedback_left;
using TestManagingService.Application.Common.interfaces.repositories.feedback_records;
using TestManagingService.Domain.FeedbackRecordAggregate.general_test;

namespace TestManagingService.Application.TestFeedbackRecords.integration_events;

public class FeedbackForGeneralTestLeftIntegrationEventHandler 
    : INotificationHandler<FeedbackForGeneralTestLeftIntegrationEvent>
{
    private readonly IGeneralTestFeedbackRecordsRepository _generalTestFeedbackRecordsRepository;

    public FeedbackForGeneralTestLeftIntegrationEventHandler(
        IGeneralTestFeedbackRecordsRepository generalTestFeedbackRecordsRepository
    ) {
        _generalTestFeedbackRecordsRepository = generalTestFeedbackRecordsRepository;
    }

    public async Task Handle(
        FeedbackForGeneralTestLeftIntegrationEvent notification, CancellationToken cancellationToken
    ) {
        var record = notification.WasLeftAnonymously
            ? GeneralTestFeedbackRecord.CreateNewAnonymous(
                notification.TestId, notification.CreatedOn, notification.Text
            )
            : GeneralTestFeedbackRecord.CreateNewNonAnonymous(
                notification.TestId, notification.AuthorId, notification.CreatedOn, notification.Text
            );
        await _generalTestFeedbackRecordsRepository.Add(record);
    }
}