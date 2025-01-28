using MediatR;
using SharedKernel.Common.errors;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.GeneralTestQuestionAggregate;
using TestCreationService.Domain.TestAggregate.general_format;
using TestCreationService.Domain.TestAggregate.general_format.events;

namespace TestCreationService.Application.Tests.general_format.events;

internal class GeneralTestResultDeletedEventHandler : INotificationHandler<GeneralTestResultDeletedEvent>
{
    private readonly IGeneralFormatTestsRepository _generalFormatTestsRepository;
    private readonly IGeneralTestQuestionsRepository _generalTestQuestionsRepository;

    public GeneralTestResultDeletedEventHandler(
        IGeneralFormatTestsRepository generalFormatTestsRepository,
        IGeneralTestQuestionsRepository generalTestQuestionsRepository
    ) {
        _generalFormatTestsRepository = generalFormatTestsRepository;
        _generalTestQuestionsRepository = generalTestQuestionsRepository;
    }

    public async Task Handle(GeneralTestResultDeletedEvent notification, CancellationToken cancellationToken) {
        GeneralFormatTest? test = await _generalFormatTestsRepository.GetById(notification.TestId);
        if (test is null) {
            throw new ErrCausedException(Err.ErrPresets.GeneralTestNotFound(notification.TestId));
        }
        foreach (var qId in test.GetTestQuestionIds()) {
            GeneralTestQuestion? question = await _generalTestQuestionsRepository.GetWithAnswers(qId);
            if (question is null) { continue; }
            question.RemoveRelatedResultFromAnswers(notification.ResultId);
        }

    }
}
