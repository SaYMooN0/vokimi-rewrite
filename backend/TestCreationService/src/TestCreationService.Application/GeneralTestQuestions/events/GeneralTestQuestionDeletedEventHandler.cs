using MediatR;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.GeneralTestQuestionAggregate.events;

namespace TestCreationService.Application.GeneralTestQuestions.events;
internal class GeneralTestQuestionDeletedEventHandler : INotificationHandler<GeneralTestQuestionDeletedEvent>
{
    private readonly IGeneralTestQuestionsRepository _generalTestQuestionsRepository;

    public GeneralTestQuestionDeletedEventHandler(IGeneralTestQuestionsRepository generalTestQuestionsRepository) {
        _generalTestQuestionsRepository = generalTestQuestionsRepository;
    }

    public async Task Handle(GeneralTestQuestionDeletedEvent notification, CancellationToken cancellationToken) {
        await _generalTestQuestionsRepository.DeleteById(notification.QuestionId);
    }
}
