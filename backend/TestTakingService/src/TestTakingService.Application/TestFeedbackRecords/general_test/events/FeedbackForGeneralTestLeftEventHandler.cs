using MediatR;
using SharedKernel.Common.errors;
using TestTakingService.Application.Common.interfaces.repositories.feedback_records;
using TestTakingService.Domain.TestFeedbackRecordAggregate.events;
using TestTakingService.Domain.TestFeedbackRecordAggregate.general_test;

namespace TestTakingService.Application.TestFeedbackRecords.general_test.events;

public class FeedbackForGeneralTestLeftEventHandler : INotificationHandler<FeedbackForGeneralTestLeftEvent>
{
    private readonly IGeneralTestFeedbackRecordsRepository _generalTestFeedbackRecordsRepository;

    public FeedbackForGeneralTestLeftEventHandler(
        IGeneralTestFeedbackRecordsRepository generalTestFeedbackRecordsRepository) {
        _generalTestFeedbackRecordsRepository = generalTestFeedbackRecordsRepository;
    }

    public async Task Handle(FeedbackForGeneralTestLeftEvent notification, CancellationToken cancellationToken) {
        var creationRes = GeneralTestFeedbackRecord.CreateNew(
            notification.FeedbackRecordId,
            notification.TestId,
            notification.AppUserId,
            notification.TestTakenRecordId,
            notification.CreatedOn,
            notification.Text,
            notification.WasLeftAnonymously
        );
        if (creationRes.IsErr(out var err)) {
            throw new ErrCausedException(err);
        }

        await _generalTestFeedbackRecordsRepository.Add(creationRes.GetSuccess());
    }
}