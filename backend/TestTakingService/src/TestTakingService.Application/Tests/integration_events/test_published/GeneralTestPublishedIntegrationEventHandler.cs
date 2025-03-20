using System.Collections.Immutable;
using SharedKernel.IntegrationEvents.test_publishing;
using TestTakingService.Application.Common.interfaces.repositories.tests;
using TestTakingService.Domain.TestAggregate.general_format;

namespace TestTakingService.Application.Tests.integration_events.test_published;

internal class GeneralTestPublishedIntegrationEventHandler
    : TestPublishedIntegrationEventHandler<GeneralTestPublishedIntegrationEvent>
{
    private IGeneralFormatTestsRepository _generalFormatTestsRepository;

    public GeneralTestPublishedIntegrationEventHandler(IGeneralFormatTestsRepository generalFormatTestsRepository) {
        _generalFormatTestsRepository = generalFormatTestsRepository;
    }

    public override async Task Handle(
        GeneralTestPublishedIntegrationEvent notification, CancellationToken cancellationToken
    ) {
        var results = CreateResultsFromNotification(notification);
        GeneralFormatTest test = new(
            notification.TestId,
            notification.CreatorId,
            notification.EditorIds.ToImmutableHashSet(),
            notification.InteractionsAccessSettings.TestAccess,
            CreateStyleFromNotification(notification),
            CreateQuestionsFromNotification(notification, results),
            notification.ShuffleQuestions,
            results,
            notification.FeedbackOption
        );
        await _generalFormatTestsRepository.Add(test);
    }

    private GeneralTestResult[] CreateResultsFromNotification(
        GeneralTestPublishedIntegrationEvent notification
    ) => notification.Results.Select(r =>
        new GeneralTestResult(r.Id, r.Name, r.Text, r.Image)
    ).ToArray();

    private GeneralTestQuestion[] CreateQuestionsFromNotification(
        GeneralTestPublishedIntegrationEvent notification,
        GeneralTestResult[] allResults
    ) => notification.Questions.Select(q => new GeneralTestQuestion(
        q.Id,
        q.Order,
        q.Text,
        q.Images.ToArray(),
        q.AnswersType,
        q.Answers.Select(a =>
            new GeneralTestAnswer(
                a.Id,
                a.TypeSpecificData,
                allResults.Where(r => a.RelatedResultsIds.Contains(r.Id)).ToArray()
            )
        ).ToArray(),
        q.ShuffleAnswers,
        q.AnswersCountLimit,
        q.TimeLimitOption
    )).ToArray();
}