using MediatR;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests.general_format;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.TestAggregate.general_format;

namespace TestCreationService.Application.Tests.general_format.commands;

public record class UpdateGeneralTestFeedbackCommand(
    TestId TestId,
    GeneralTestFeedbackOption FeedbackOption
) : IRequest<ErrOrNothing>;

public class UpdateGeneralTestFeedbackCommandHandler
    : IRequestHandler<UpdateGeneralTestFeedbackCommand, ErrOrNothing>
{
    private readonly IGeneralFormatTestsRepository _generalFormatTestsRepository;

    public UpdateGeneralTestFeedbackCommandHandler(IGeneralFormatTestsRepository generalFormatTestsRepository) {
        _generalFormatTestsRepository = generalFormatTestsRepository;
    }

    public async Task<ErrOrNothing> Handle(
        UpdateGeneralTestFeedbackCommand request,
        CancellationToken cancellationToken
    ) {
        GeneralFormatTest? test = await _generalFormatTestsRepository.GetById(request.TestId);
        if (test is null) {
            return Err.ErrPresets.TestNotFound(request.TestId);
        }

        test.UpdateTestFeedback(request.FeedbackOption);
        await _generalFormatTestsRepository.Update(test);
        return ErrOrNothing.Nothing;
    }
}