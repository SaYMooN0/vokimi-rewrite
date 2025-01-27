using MediatR;
using SharedKernel.Common.errors;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.TestAggregate.general_format;
using TestCreationService.Domain.TestAggregate.general_format.events;

namespace TestCreationService.Application.Tests.general_format.events;

internal class RelatedResultsForGeneralTestAnswerChangedEventHandler : INotificationHandler<RelatedResultsForGeneralTestAnswerChangedEvent>
{
    private readonly IGeneralFormatTestsRepository _generalFormatTestsRepository;

    public RelatedResultsForGeneralTestAnswerChangedEventHandler(IGeneralFormatTestsRepository generalFormatTestsRepository) {
        _generalFormatTestsRepository = generalFormatTestsRepository;
    }

    public async Task Handle(RelatedResultsForGeneralTestAnswerChangedEvent notification, CancellationToken cancellationToken) {
        GeneralFormatTest? test = await _generalFormatTestsRepository.GetWithResults(notification.TestId);
        if (test is null) {
            throw new ErrCausedException(Err.ErrPresets.GeneralTestNotFound(notification.TestId));
        }
        foreach (var r in notification.RelatedResults) {
            if (!test.HasResultWithId(r)) {
                throw new ErrCausedException(
                    new Err(
                        "This general format test doesn't have selected result",
                        details: $"Test id: {test.Id}, result id: {r}"
                    )
                );
            }
        }

    }
}
