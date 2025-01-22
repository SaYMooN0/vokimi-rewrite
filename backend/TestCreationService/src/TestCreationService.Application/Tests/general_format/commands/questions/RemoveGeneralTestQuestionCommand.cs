using MediatR;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.TestAggregate.general_format;

namespace TestCreationService.Application.Tests.general_format.commands.questions;

public record class RemoveGeneralTestQuestionCommand(
    TestId TestId,
    GeneralTestQuestionId QuestionId
) : IRequest<ErrOrNothing>;
public class RemoveGeneralTestQuestionCommandHandler : IRequestHandler<RemoveGeneralTestQuestionCommand, ErrOrNothing>
{
    private readonly IGeneralFormatTestsRepository generalFormatTestsRepository;

    public RemoveGeneralTestQuestionCommandHandler(IGeneralFormatTestsRepository generalFormatTestsRepository) {
        this.generalFormatTestsRepository = generalFormatTestsRepository;
    }

    public async Task<ErrOrNothing> Handle(RemoveGeneralTestQuestionCommand request, CancellationToken cancellationToken) {
        GeneralFormatTest? test = await generalFormatTestsRepository.GetWithQuestions(request.TestId);
        if (test is null) {
            return Err.ErrPresets.GeneralTestNotFound(request.TestId);
        }
        var removingRes = test.RemoveQuestion(request.QuestionId);
        if (removingRes.IsErr(out var err)) { return err; }
        await generalFormatTestsRepository.Update(test);
        return ErrOrNothing.Nothing;
    }
}
