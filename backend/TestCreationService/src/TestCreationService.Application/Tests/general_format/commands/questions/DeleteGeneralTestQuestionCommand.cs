using MediatR;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.TestAggregate.general_format;

namespace TestCreationService.Application.Tests.general_format.commands.questions;

public record class DeleteGeneralTestQuestionCommand(
    TestId TestId,
    GeneralTestQuestionId QuestionId
) : IRequest<ErrOrNothing>;
internal class DeleteGeneralTestQuestionCommandHandler : IRequestHandler<DeleteGeneralTestQuestionCommand, ErrOrNothing>
{
    private readonly IGeneralFormatTestsRepository _generalFormatTestsRepository;

    public DeleteGeneralTestQuestionCommandHandler(IGeneralFormatTestsRepository generalFormatTestsRepository) {
        _generalFormatTestsRepository = generalFormatTestsRepository;
    }

    public async Task<ErrOrNothing> Handle(DeleteGeneralTestQuestionCommand request, CancellationToken cancellationToken) {
        GeneralFormatTest? test = await _generalFormatTestsRepository.GetById(request.TestId);
        if (test is null) {
            return Err.ErrPresets.GeneralTestNotFound(request.TestId);
        }
        var deletingRes=test.DeleteQuestion(request.QuestionId);
        if(deletingRes.IsErr(out var err)) {
            return err;
        }
        return ErrOrNothing.Nothing;
    }
}
