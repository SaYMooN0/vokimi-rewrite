using MediatR;
using SharedKernel.Common.errors;
using SharedKernel.Common.general_test_questions;
using SharedKernel.IntegrationEvents.test_publishing;
using System.Collections.Immutable;
using TestCatalogService.Application.Common.interfaces.repositories;
using TestCatalogService.Application.Common.interfaces.repositories.tests;
using TestCatalogService.Domain.Common;
using TestCatalogService.Domain.TestAggregate.formats_shared;
using TestCatalogService.Domain.TestAggregate.general_format;

namespace TestCatalogService.Application.Tests.integration_events;

internal class GeneralTestPublishedIntegrationEventHandler : INotificationHandler<GeneralTestPublishedIntegrationEvent>
{
    private IGeneralFormatTestsRepository _generalFormatTestsRepository;

    public GeneralTestPublishedIntegrationEventHandler(IGeneralFormatTestsRepository generalFormatTestsRepository) {
        _generalFormatTestsRepository = generalFormatTestsRepository;
    }

    public async Task Handle(GeneralTestPublishedIntegrationEvent notification, CancellationToken cancellationToken) {
        var questionsCount = (ushort)notification.Questions.Count();
        var resultsCount = (ushort)notification.Results.Count();
        bool anyAudioAnswers = notification.Questions.Any(q => q.AnswersType.HasAudio());
        var interactionsAccessSettings = GetInteractionsAccessSettingsFromNotification(notification);
        var tags = GetTagsFromNotification(notification);
        var creationRes = GeneralFormatTest.CreateNew(
            notification.TestId,
            notification.Name,
            notification.CoverImage,
            notification.Description,
            notification.CreatorId,
            notification.EditorIds,
            notification.PublicationDate,
            notification.Language,
            questionsCount: questionsCount,
            resultsCount: resultsCount,
            anyAudioAnswers: anyAudioAnswers,
            interactionsAccessSettings,
            tags
        );
        if (creationRes.IsErr(out var err)) {
            throw new ErrCausedException(err);
        }

        await _generalFormatTestsRepository.Add(creationRes.GetSuccess());
    }

    private static ImmutableHashSet<TestTagId> GetTagsFromNotification(
        GeneralTestPublishedIntegrationEvent notification
    ) => notification.Tags
        .Select(t => {
            var res = TestTagId.Create(t);
            if (res.IsErr(out var err)) {
                throw new ErrCausedException(err);
            }

            return res.GetSuccess();
        })
        .ToImmutableHashSet();

    private static TestInteractionsAccessSettings GetInteractionsAccessSettingsFromNotification(
        GeneralTestPublishedIntegrationEvent notification
    ) => new(
        testAccess: notification.InteractionsAccessSettings.TestAccess,
        allowRatings: notification.InteractionsAccessSettings.AllowRatings,
        allowComments: notification.InteractionsAccessSettings.AllowComments,
        allowTestTakenPosts: notification.InteractionsAccessSettings.AllowTestTakenPosts,
        allowTagsSuggestions: notification.InteractionsAccessSettings.AllowTagsSuggestions
    );
}