using MediatR;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests.general_format;
using TestManagingService.Application.Common.interfaces.repositories.tests;
using TestManagingService.Domain.TestAggregate.general_format;

namespace TestManagingService.Application.Tests.general_format.feedback;

public record DisableFeedbackForGeneralTestCommand(
    TestId TestId
) : IRequest<ErrOr<GeneralTestFeedbackOption>>;

internal class DisableFeedbackForGeneralTestCommandHandler
    : IRequestHandler<DisableFeedbackForGeneralTestCommand, ErrOr<GeneralTestFeedbackOption>>
{
    private readonly IGeneralFormatTestsRepository _generalFormatTestsRepository;

    public DisableFeedbackForGeneralTestCommandHandler(
        IGeneralFormatTestsRepository generalFormatTestsRepository
    ) {
        _generalFormatTestsRepository = generalFormatTestsRepository;
    }

    public async Task<ErrOr<GeneralTestFeedbackOption>> Handle(
        DisableFeedbackForGeneralTestCommand request,
        CancellationToken cancellationToken
    ) {
        GeneralFormatTest? test = await _generalFormatTestsRepository.GetById(request.TestId);
        if (test is null) {
            return Err.ErrPresets.TestNotFound(request.TestId);
        }

        test.SetFeedbackOptionDisabled();

        await _generalFormatTestsRepository.Update(test);
        return test.FeedbackOption;
    }
}