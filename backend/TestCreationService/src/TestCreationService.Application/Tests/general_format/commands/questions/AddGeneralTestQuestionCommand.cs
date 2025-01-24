using MediatR;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests.general_format_tests;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Application.Common.interfaces.repositories.general_format_tests;
using TestCreationService.Domain.Common.rules;
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
        GeneralFormatTest? test = await _generalFormatTestsRepository.GetWithQuestions(request.TestId);
        if (test is null) {
            return Err.ErrPresets.GeneralTestNotFound(request.TestId);
        }
        var addingRes = test.AddNewQuestion(request.AnswersType);
        if (addingRes.IsErr(out var err)) { return err; }
        await _generalFormatTestsRepository.Update(test);
        return ErrOrNothing.Nothing;
    }
}
