using System.Collections.Immutable;
using MediatR;
using SharedKernel.Common.tests.formats_shared.test_styles;
using SharedKernel.IntegrationEvents.test_publishing;
using TestTakingService.Application.Common.interfaces.repositories.tests;

namespace TestTakingService.Application.Tests.integration_events;

internal class TierListTestPublishedIntegrationEventHandler : INotificationHandler<TierListTestPublishedIntegrationEvent>
{
    private ITierListFormatTestsRepository _TierListFormatTestsRepository;

    public TierListTestPublishedIntegrationEventHandler(ITierListFormatTestsRepository TierListFormatTestsRepository) {
        _TierListFormatTestsRepository = TierListFormatTestsRepository;
    }

    public async Task Handle(TierListTestPublishedIntegrationEvent notification, CancellationToken cancellationToken) {
        var results = CreateResultsFromNotification(notification);
        var questions = CreateQuestionsFromNotification(notification, results);
        var styles = CreateStyleFromNotification(notification);

        TierListFormatTest test = new TierListFormatTest(
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
        await _TierListFormatTestsRepository.Add(test);
    }

    private TierListTestResult[] CreateResultsFromNotification(
        TierListTestPublishedIntegrationEvent notification
    ) => notification.Results.Select(r => new TierListTestResult(
        r.Id,
        r.Name,
        r.Text,
        r.Image
    )).ToArray();

    private TierListTestQuestion[] CreateQuestionsFromNotification(
        TierListTestPublishedIntegrationEvent notification,
        TierListTestResult[] allResults
    ) => notification.Questions.Select(q => new TierListTestQuestion(
        q.Id,
        q.Order,
        q.Text,
        q.Images.ToArray(),
        q.AnswersType,
        q.Answers.Select(a =>
            new TierListTestAnswer(
                a.Id,
                a.TypeSpecificData,
                allResults.Where(r => a.RelatedResultsIds.Contains(r.Id)).ToArray()
            )
        ).ToArray(),
        q.ShuffleAnswers,
        q.AnswersCountLimit,
        q.TimeLimitOption
    )).ToArray();

    private TestStylesSheet CreateStyleFromNotification(TierListTestPublishedIntegrationEvent notification) => new(
        notification.Styles.Id,
        notification.TestId,
        notification.Styles.AccentColor,
        notification.Styles.ErrorsColor,
        notification.Styles.Buttons
    );
}