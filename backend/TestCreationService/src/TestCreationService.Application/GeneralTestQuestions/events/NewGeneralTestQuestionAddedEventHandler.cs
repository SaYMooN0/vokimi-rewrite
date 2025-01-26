using MediatR;
using TestCreationService.Domain.GeneralTestQuestionAggregate.events;

namespace TestCreationService.Application.GeneralTestQuestions.events;
internal class NewGeneralTestQuestionAddedEventHandler : INotificationHandler<NewGeneralTestQuestionAddedEvent>
{
    public Task Handle(NewGeneralTestQuestionAddedEvent notification, CancellationToken cancellationToken) {
    
    } 
}
