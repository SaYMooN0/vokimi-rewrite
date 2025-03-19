using System.Collections.Immutable;
using MediatR;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.general_test_questions;
using SharedKernel.IntegrationEvents.test_publishing;
using TestCatalogService.Domain.Common.interfaces.repositories.tests;
using TestCatalogService.Domain.TestAggregate.general_format;

namespace TestCatalogService.Application.Tests.integration_events.test_published;

internal class GeneralTestPublishedIntegrationEventHandler
    : TestPublishedIntegrationEventHandler<GeneralTestPublishedIntegrationEvent>
{
    private IGeneralFormatTestsRepository _generalFormatTestsRepository;

    public GeneralTestPublishedIntegrationEventHandler(
        IGeneralFormatTestsRepository generalFormatTestsRepository
    ) {
        _generalFormatTestsRepository = generalFormatTestsRepository;
    }

    public override async Task Handle(GeneralTestPublishedIntegrationEvent notification,
        CancellationToken cancellationToken) {
        var questionsCount = (ushort)notification.Questions.Count();
        var resultsCount = (ushort)notification.Results.Count();
        bool anyAudioAnswers = notification.Questions.Any(q => q.AnswersType.HasAudio());
        var interactionsAccessSettings = GetInteractionsAccessSettingsFromNotification(notification);
        var tags = GetTagsFromNotification(notification);
        GeneralFormatTest newTest = new(
            notification.TestId,
            notification.Name,
            notification.CoverImage,
            notification.Description,
            notification.CreatorId,
            notification.EditorIds,
            notification.PublicationDate,
            notification.Language,
            tags,
            interactionsAccessSettings,
            questionsCount: questionsCount,
            resultsCount: resultsCount,
            anyAudioAnswers: anyAudioAnswers
        );


        await _generalFormatTestsRepository.Add(newTest);
    }
}