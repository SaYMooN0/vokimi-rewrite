using MediatR;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity_id;
using SharedKernel.Common.errors;
using SharedKernel.Common.general_test_questions.answer_type_specific_data;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.GeneralTestQuestionAggregate;

namespace TestCreationService.Application.GeneralTestQuestions.commands.answers;


public record class UpdateAnswerForGeneralTestQuestionCommand(
    GeneralTestQuestionId QuestionId,
    GeneralTestAnswerId AnswerId,
    GeneralTestAnswerTypeSpecificData AnswerData,
    HashSet<GeneralTestResultId> RelatedResultIds
) : IRequest<ErrOrNothing>;

internal class UpdateAnswerForGeneralTestQuestionCommandHandler : IRequestHandler<UpdateAnswerForGeneralTestQuestionCommand, ErrOrNothing>
{
    private readonly IGeneralTestQuestionsRepository _questionsRepository;

    public UpdateAnswerForGeneralTestQuestionCommandHandler(IGeneralTestQuestionsRepository questionsRepository) {
        _questionsRepository = questionsRepository;
    }

    public async Task<ErrOrNothing> Handle(UpdateAnswerForGeneralTestQuestionCommand request, CancellationToken cancellationToken) {
        GeneralTestQuestion? question = await _questionsRepository.GetWithAnswers(request.QuestionId);
        if (question is null) {
            return Err.ErrPresets.GeneralTestQuestionNotFound(request.QuestionId);
        }

        var updatingRes = question.UpdateAnswer(request.AnswerId, request.AnswerData, request.RelatedResultIds);
        if (updatingRes.IsErr(out var err)) {
            return err;
        }

        await _questionsRepository.Update(question);
        return ErrOrNothing.Nothing;
    }
}
