using MediatR;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.GeneralTestQuestionAggregate;
using TestCreationService.Domain.GeneralTestQuestionAggregate.events;

namespace TestCreationService.Application.GeneralTestQuestions.events;
internal class NewGeneralTestQuestionAddedEventHandler : INotificationHandler<NewGeneralTestQuestionAddedEvent>
{
    private readonly IGeneralTestQuestionsRepository _generalTestQuestionsRepository;

    public NewGeneralTestQuestionAddedEventHandler(IGeneralTestQuestionsRepository generalTestQuestionsRepository) {
        _generalTestQuestionsRepository = generalTestQuestionsRepository;
    }

    public async Task Handle(NewGeneralTestQuestionAddedEvent notification, CancellationToken cancellationToken) {
        GeneralTestQuestion q = new(notification.QuestionId, notification.TestId, notification.AnswersType);
        await _generalTestQuestionsRepository.Add(q);
    }
}
