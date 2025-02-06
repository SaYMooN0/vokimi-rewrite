using System.Collections.Immutable;
using MediatR;
using SharedKernel.IntegrationEvents.test_publishing;
using TestTakingService.Application.Common.interfaces.repositories;
using TestTakingService.Domain.TestAggregate.general_format;

namespace TestTakingService.Application.Tests.integration_events;

internal class GeneralTestPublishedIntegrationEventHandler : INotificationHandler<GeneralTestPublishedIntegrationEvent>
{
    private IGeneralFormatTestsRepository _generalFormatTestsRepository;

    public GeneralTestPublishedIntegrationEventHandler(IGeneralFormatTestsRepository generalFormatTestsRepository) {
        _generalFormatTestsRepository = generalFormatTestsRepository;
    }

    public async Task Handle(GeneralTestPublishedIntegrationEvent notification, CancellationToken cancellationToken) {
        var results = CreateResultsFromNotification(notification);
        var questions = CreateQuestionsFromNotification(notification, results);

        GeneralFormatTest test = new GeneralFormatTest(
            notification.TestId,
            notification.CreatorId,
            questions,
            notification.ShuffleQuestions,
            results,
            notification.FeedbackOption
        );
        await _generalFormatTestsRepository.Add(test);
    }

    private ImmutableArray<GeneralTestResult> CreateResultsFromNotification(
        GeneralTestPublishedIntegrationEvent notification
    ) => notification.Results.Select(r => new GeneralTestResult(
        r.Id,
        notification.TestId,
        r.Name,
        r.Text,
        r.Image
    )).ToImmutableArray();

    private ImmutableArray<GeneralTestQuestion> CreateQuestionsFromNotification(
        GeneralTestPublishedIntegrationEvent notification,
        ImmutableArray<GeneralTestResult> allResults
    ) => notification.Questions.Select(q => new GeneralTestQuestion(
        q.Id,
        notification.TestId,
        q.Order,
        q.Text,
        q.Images.ToImmutableArray(),
        q.Answers.Select(a =>
            new GeneralTestAnswer(
                a.Id,
                q.Id,
                a.TypeSpecificData,
                allResults.Where(r => a.RelatedResultsIds.Contains(r.Id)).ToImmutableArray()
            )
        ).ToImmutableArray(),
        q.ShuffleAnswers,
        q.AnswersCountLimit,
        q.TimeLimitOption
    )).ToImmutableArray();
}