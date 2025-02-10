using MediatR;
using TestTakingService.Domain.TestTakenRecordAggregate.general_test.events;

namespace TestTakingService.Application.TestTakenRecords.general_test.events;

public class GeneralTestTakenEventHandler: INotificationHandler<GeneralTestTakenEvent>
{
    public async Task Handle(GeneralTestTakenEvent notification, CancellationToken cancellationToken) {
        ...
    }