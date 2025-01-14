using MediatR;
using SharedKernel.Common;
using TestCreationService.Domain.TestAggregate.formats_shared.events;

namespace TestCreationService.Application.Tests.formats_shared.events;

internal class TestEditorsListChangedEventHandler : INotificationHandler<TestEditorsListChangedEvent>
{
    public async Task Handle(TestEditorsListChangedEvent notification, CancellationToken cancellationToken) {
       
    }
}
