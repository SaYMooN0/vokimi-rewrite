using MediatR;
using SharedKernel.Common.domain;
using SharedKernel.Common.errors;
using SharedKernel.Common.general_test_questions;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.TestAggregate.general_format;

namespace TestCreationService.Application.Tests.general_format.commands.questions;

public record class AddGeneralTestQuestionCommand(
    TestId TestId,
    GeneralTestAnswersType AnswersType
) : IRequest<ErrOrNothing>;
internal class AddGeneralTestQuestionCommandHandler : IRequestHandler<AddGeneralTestQuestionCommand, ErrOrNothing>
{
    private readonly IGeneralFormatTestsRepository _generalFormatTestsRepository;

    public AddGeneralTestQuestionCommandHandler(IGeneralFormatTestsRepository generalFormatTestsRepository) {
        _generalFormatTestsRepository = generalFormatTestsRepository;
    }

    public async Task<ErrOrNothing> Handle(AddGeneralTestQuestionCommand request, CancellationToken cancellationToken) {
        GeneralFormatTest? test = await _generalFormatTestsRepository.GetById(request.TestId);
        if (test is null) {
            return Err.ErrPresets.GeneralTestNotFound(request.TestId);
        }
        var addingRes = test.AddNewQuestion(request.AnswersType);
        if (addingRes.IsErr(out var err)) { return err; }
        await _generalFormatTestsRepository.Update(test);
        return ErrOrNothing.Nothing;
    }
}
