using MediatR;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests.general_format;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.TestAggregate.general_format;

namespace TestCreationService.Application.Tests.general_format.commands;

public record class GetGeneralTestFeedbackOptionCommand(
    TestId TestId
) : IRequest<ErrOr<GeneralTestFeedbackOption>>;

public class GetGeneralTestFeedbackOptionCommandHandler
    : IRequestHandler<GetGeneralTestFeedbackOptionCommand, ErrOr<GeneralTestFeedbackOption>>
{
    private readonly IGeneralFormatTestsRepository _generalFormatTestsRepository;

    public GetGeneralTestFeedbackOptionCommandHandler(
        IGeneralFormatTestsRepository generalFormatTestsRepository
    ) {
        _generalFormatTestsRepository = generalFormatTestsRepository;
    }

    public async Task<ErrOr<GeneralTestFeedbackOption>> Handle(
        GetGeneralTestFeedbackOptionCommand request, CancellationToken cancellationToken
    ) {
        GeneralFormatTest? test = await _generalFormatTestsRepository.GetById(request.TestId);
        if (test is null) {
            return Err.ErrPresets.TestNotFound(request.TestId);
        }

        return test.Feedback;
    }
}