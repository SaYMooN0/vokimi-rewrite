using MediatR;
using SharedKernel.Common.domain;
using SharedKernel.Common.errors;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.Common;
using TestCreationService.Domain.GeneralTestQuestionAggregate;

namespace TestCreationService.Application.GeneralTestQuestions.commands.answers;

public record class UpdateAnswersOrderInGeneralTestQuestionCommand(
    GeneralTestQuestionId QuestionId,
    EntitiesOrderController<GeneralTestAnswerId> OrderController
) : IRequest<ErrOrNothing>;
internal class UpdateAnswersOrderInGeneralTestQuestionCommandHandler : IRequestHandler<UpdateAnswersOrderInGeneralTestQuestionCommand, ErrOrNothing>
{
    private readonly IGeneralTestQuestionsRepository _questionsRepository;

    public UpdateAnswersOrderInGeneralTestQuestionCommandHandler(IGeneralTestQuestionsRepository questionsRepository) {
        _questionsRepository = questionsRepository;
    }

    public async Task<ErrOrNothing> Handle(UpdateAnswersOrderInGeneralTestQuestionCommand request, CancellationToken cancellationToken) {
        GeneralTestQuestion? question = await _questionsRepository.GetWithAnswers(request.QuestionId);
        if (question is null) {
            return Err.ErrPresets.GeneralTestQuestionNotFound(request.QuestionId);
        }
        var updateErr = question.UpdateAnswerOrder(request.OrderController);
        if (updateErr.IsErr(out var err)) { return err; }
        await _questionsRepository.Update(question);
        return ErrOrNothing.Nothing;
    }
}
