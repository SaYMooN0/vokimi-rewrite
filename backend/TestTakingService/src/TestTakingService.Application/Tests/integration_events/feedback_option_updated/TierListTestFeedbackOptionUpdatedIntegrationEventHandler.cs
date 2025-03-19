using MediatR;
using SharedKernel.Common.errors;
using SharedKernel.IntegrationEvents.test_managing.feedback_option_updated;
using TestTakingService.Application.Common.interfaces.repositories.tests;

namespace TestTakingService.Application.Tests.integration_events.feedback_option_updated;


public class TierListTestFeedbackOptionUpdatedIntegrationEventHandler
    : INotificationHandler<TierListTestFeedbackOptionUpdatedIntegrationEvent>
{
    private readonly ITierListFormatTestsRepository _tierListFormatRepository;

    public TierListTestFeedbackOptionUpdatedIntegrationEventHandler(
        ITierListFormatTestsRepository tierListFormatRepository
    ) {
        _tierListFormatRepository = tierListFormatRepository;
    }

    public async Task Handle(
        TierListTestFeedbackOptionUpdatedIntegrationEvent notification,
        CancellationToken cancellationToken
    ) {
        TierListFormatTest? test = await _tierListFormatRepository.GetById(notification.TestId);
        if (test is null) {
            throw new ErrCausedException(Err.ErrPresets.TierListTestNotFound(notification.TestId));
        }

        test.UpdateFeedbackOption(notification.NewFeedbackOption);
        await _tierListFormatRepository.Update(test);
    }
}