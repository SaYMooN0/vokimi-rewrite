using MediatR;
using TestCreationService.Domain.TestAggregate.general_format.events;

namespace TestCreationService.Application.Tests.general_format.events;

internal class RelatedResultsForGeneralTestAnswerChangedEventHandler : INotificationHandler<RelatedResultsForGeneralTestAnswerChangedEvent>
{
    public Task Handle(RelatedResultsForGeneralTestAnswerChangedEvent notification, CancellationToken cancellationToken) {
    
    }
}
