using MediatR;
using TestTakingService.Domain.TestFeedbackRecordAggregate.general_test.events;

namespace TestTakingService.Application.TestFeedbackRecords.general_test.events;

public class FeedbackForGeneralTestLeftEventHandler: INotificationHandler<FeedbackForGeneralTestLeftEvent>
{
    public async Task Handle(FeedbackForGeneralTestLeftEvent notification, CancellationToken cancellationToken) {
        ...
    }
}