using MediatR;
using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests.general_format;
using TestManagingService.Application.Common.interfaces.repositories.tests;
using TestManagingService.Domain.TestAggregate.general_format;

namespace TestManagingService.Application.Tests.general_format.feedback;

public record EnableFeedbackForGeneralTestCommand(
    TestId TestId,
    AnonymityValues Anonymity,
    string AccompanyingText,
    ushort MaxFeedbackLength
) : IRequest<ErrOr<GeneralTestFeedbackOption>>;

internal class EnableFeedbackForGeneralTestCommandHandler
    : IRequestHandler<EnableFeedbackForGeneralTestCommand, ErrOr<GeneralTestFeedbackOption>>
{
    private readonly IGeneralFormatTestsRepository _generalFormatTestsRepository;

    public EnableFeedbackForGeneralTestCommandHandler(
        IGeneralFormatTestsRepository generalFormatTestsRepository
    ) {
        _generalFormatTestsRepository = generalFormatTestsRepository;
    }

    public async Task<ErrOr<GeneralTestFeedbackOption>> Handle(
        EnableFeedbackForGeneralTestCommand request,
        CancellationToken cancellationToken
    ) {
        GeneralFormatTest? test = await _generalFormatTestsRepository.GetById(request.TestId);
        if (test is null) {
            return Err.ErrPresets.TestNotFound(request.TestId);
        }

        var result = test.SetFeedbackOptionEnabled(
            request.Anonymity,
            request.AccompanyingText,
            request.MaxFeedbackLength
        );
        if (result.IsErr(out var err)) {
            return err;
        }

        await _generalFormatTestsRepository.Update(test);
        return test.FeedbackOption;
    }
}