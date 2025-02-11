using System.Collections.Immutable;
using MediatR;
using SharedKernel.Common.tests.test_styles;
using SharedKernel.IntegrationEvents.test_publishing;
using TestTakingService.Application.Common.interfaces.repositories;
using TestTakingService.Application.Common.interfaces.repositories.tests;
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
        var styles = CreateStyleFromNotification(notification);

        GeneralFormatTest test = new GeneralFormatTest(
            notification.TestId,
            notification.CreatorId,
            notification.EditorIds.ToImmutableHashSet(),
            notification.InteractionsAccessSettings.TestAccess,
            styles,
            questions,
            notification.ShuffleQuestions,
            results,
            notification.FeedbackOption
        );
        await _generalFormatTestsRepository.Add(test);
    }

    private GeneralTestResult[] CreateResultsFromNotification(
        GeneralTestPublishedIntegrationEvent notification
    ) => notification.Results.Select(r => new GeneralTestResult(
        r.Id,
        r.Name,
        r.Text,
        r.Image
    )).ToArray();

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

    private TestStylesSheet CreateStyleFromNotification(GeneralTestPublishedIntegrationEvent notification) => new(
        notification.Styles.Id,
        notification.TestId,
        notification.Styles.AccentColor,
        notification.Styles.ErrorsColor,
        notification.Styles.Buttons
    );
}