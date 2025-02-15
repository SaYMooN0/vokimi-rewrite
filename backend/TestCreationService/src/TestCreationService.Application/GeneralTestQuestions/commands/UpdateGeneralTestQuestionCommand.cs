using MediatR;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity_id;
using SharedKernel.Common.errors;
using SharedKernel.Common.general_test_questions;
using SharedKernel.Common.general_test_questions.answer_type_specific_data;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.GeneralTestQuestionAggregate;

namespace TestCreationService.Application.GeneralTestQuestions.commands;

public record class UpdateGeneralTestQuestionCommand(
    GeneralTestQuestionId QuestionId,
    string Text,
    string[] Images,
    GeneralTestQuestionTimeLimitOption TimeLimit,
    GeneralTestQuestionAnswersCountLimit AnswerCountLimit
) : IRequest<ErrListOrNothing>;
internal class UpdateGeneralTestQuestionCommandHandler : IRequestHandler<UpdateGeneralTestQuestionCommand, ErrListOrNothing>
{
    private readonly IGeneralTestQuestionsRepository _generalTestQuestionsRepository;

    public UpdateGeneralTestQuestionCommandHandler(IGeneralTestQuestionsRepository generalTestQuestionsRepository) {
        _generalTestQuestionsRepository = generalTestQuestionsRepository;
    }

    public async Task<ErrListOrNothing> Handle(UpdateGeneralTestQuestionCommand request, CancellationToken cancellationToken) {
        GeneralTestQuestion? question = await _generalTestQuestionsRepository.GetById(request.QuestionId);
        if (question is null) {
            return Err.ErrPresets.GeneralTestQuestionNotFound(request.QuestionId);
        }
        var updateRes = question.Update(
            request.Text,
            request.Images,
            request.TimeLimit,
            request.AnswerCountLimit
        );
        if (updateRes.IsErr(out var errs)) {
            return errs;
        }
        await _generalTestQuestionsRepository.Update(question);
        return ErrListOrNothing.Nothing;
    }
}
