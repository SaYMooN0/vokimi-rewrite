using MediatR;
using SharedKernel.Common.errors;
using SharedKernel.IntegrationEvents.test_managing;
using TestTakingService.Application.Common.interfaces.repositories.tests;
using TestTakingService.Domain.TestAggregate;

namespace TestTakingService.Application.Tests.integration_events;

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

        test.UpdateAccessLevel(notification.TestAccess);
        await _baseTestsRepository.Update(test);
    }
}