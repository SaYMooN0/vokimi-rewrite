using MediatR;
using TestTakingService.Application.Common.interfaces.repositories.test_taken_records;
using TestTakingService.Domain.TestTakenRecordAggregate.events;
using TestTakingService.Domain.TestTakenRecordAggregate.general_test;

namespace TestTakingService.Application.TestTakenRecords.general_test.events;

public class GeneralTestTakenEventHandler : INotificationHandler<GeneralBaseTestTakenEvent>
{
    private readonly IGeneralTestTakenRecordsRepository _generalTestTakenRecordsRepository;

    public GeneralTestTakenEventHandler(IGeneralTestTakenRecordsRepository generalTestTakenRecordsRepository) {
        _generalTestTakenRecordsRepository = generalTestTakenRecordsRepository;
    }

    public async Task Handle(GeneralBaseTestTakenEvent notification, CancellationToken cancellationToken) {
        GeneralTestTakenRecord record = GeneralTestTakenRecord.CreateNew(
            notification.AppUserId,
            notification.TestId,
            notification.TestTakingStart,
            notification.TestTakingEnd,
            notification.ReceivedResultId,
            notification.QuestionDetails.Select(idDet =>
                GeneralTestTakenRecordQuestionDetails.CreateNew(
                    idDet.Key,
                    idDet.Value.ChosenAnswerIds,
                    idDet.Value.TimeOnQuestionSpent)
            ),
            notification.Feedback
        );
        await _generalTestTakenRecordsRepository.Add(record);
    }
}