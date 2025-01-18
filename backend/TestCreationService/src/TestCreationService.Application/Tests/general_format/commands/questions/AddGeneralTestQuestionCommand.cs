using MediatR;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests.general_format_tests;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.Common.rules;
using TestCreationService.Domain.TestAggregate.general_format;

namespace TestCreationService.Application.Tests.general_format.commands.questions;

public record class AddGeneralTestQuestionCommand(
    TestId TestId,
    GeneralTestAnswersType AnswersType
) : IRequest<ErrOrNothing>;
public class AddGeneralTestQuestionCommandHandler : IRequestHandler<AddGeneralTestQuestionCommand, ErrOrNothing>
{
    private readonly IGeneralFormatTestsRepository generalFormatTestsRepository;

    public AddGeneralTestQuestionCommandHandler(IGeneralFormatTestsRepository generalFormatTestsRepository) {
        this.generalFormatTestsRepository = generalFormatTestsRepository;
    }

    public async Task<ErrOrNothing> Handle(AddGeneralTestQuestionCommand request, CancellationToken cancellationToken) {
        GeneralFormatTest? test = await generalFormatTestsRepository.GetWithQuestions(request.TestId);
        if (test is null) {
            return Err.ErrFactory.NotFound(
                "Unable to find this general format test",
                details: $"Cannot find general format test with id {request.TestId}"
            );
        }
        test.AddNewQuestion(request.AnswersType);
        await generalFormatTestsRepository.Update(test);
        return ErrOrNothing.Nothing;
    }
}
