using MediatR;
using SharedKernel.Common.errors;
using SharedKernel.IntegrationEvents.test_managing;
using TestCatalogService.Domain.Common.interfaces.repositories.tests;
using TestCatalogService.Domain.TestAggregate;

namespace TestCatalogService.Application.Tests.integration_events;

public class TestInteractionsAccessSettingsUpdatedIntegrationEventHandler
    : INotificationHandler<TestInteractionsAccessSettingsUpdatedIntegrationEvent>
{
    private readonly IBaseTestsRepository _baseTestsRepository;

    public TestInteractionsAccessSettingsUpdatedIntegrationEventHandler(
        IBaseTestsRepository baseTestsRepository
    ) {
        _baseTestsRepository = baseTestsRepository;
    }

    public async Task Handle(
        TestInteractionsAccessSettingsUpdatedIntegrationEvent notification, CancellationToken cancellationToken
    ) {
        BaseTest? test = await _baseTestsRepository.GetById(notification.TestId);
        if (test is null) {
            throw new ErrCausedException(Err.ErrPresets.TestNotFound(notification.TestId));
        }

        var updatingRes = test.UpdateInteractionsAccessSettings(
            testAccessLevel: notification.TestAccess,
            ratingsSetting: notification.AllowRatings,
            commentsSetting: notification.AllowComments,
            allowTestTakenPosts: notification.AllowTestTakenPosts,
            allowTagsSuggestions: notification.AllowTagSuggestions
        );
        if (updatingRes.IsErr(out var err)) {
            throw new ErrCausedException(err);
        }

        await _baseTestsRepository.Update(test);
    }
}