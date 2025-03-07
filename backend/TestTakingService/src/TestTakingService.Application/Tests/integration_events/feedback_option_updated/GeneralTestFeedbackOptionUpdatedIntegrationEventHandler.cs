using MediatR;
using SharedKernel.Common.errors;
using SharedKernel.IntegrationEvents.test_managing.feedback_option_updated;
using TestTakingService.Application.Common.interfaces.repositories.tests;
using TestTakingService.Domain.TestAggregate.general_format;

namespace TestTakingService.Application.Tests.integration_events.feedback_option_updated;

public class GeneralTestFeedbackOptionUpdatedIntegrationEventHandler
    : INotificationHandler<GeneralTestFeedbackOptionUpdatedIntegrationEvent>
{
    private readonly IGeneralFormatTestsRepository _generalFormatRepository;

    public GeneralTestFeedbackOptionUpdatedIntegrationEventHandler(
        IGeneralFormatTestsRepository generalFormatRepository
    ) {
        _generalFormatRepository = generalFormatRepository;
    }

    public async Task Handle(
        GeneralTestFeedbackOptionUpdatedIntegrationEvent notification,
        CancellationToken cancellationToken
    ) {
        GeneralFormatTest? test = await _generalFormatRepository.GetById(notification.TestId);
        if (test is null) {
            throw new ErrCausedException(Err.ErrPresets.GeneralTestNotFound(notification.TestId));
        }

        test.UpdateFeedbackOption(notification.NewFeedbackOption);
        await _generalFormatRepository.Update(test);
    }
}