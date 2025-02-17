using MediatR;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.general_test_questions.answer_type_specific_data;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.GeneralTestQuestionAggregate;

namespace TestCreationService.Application.GeneralTestQuestions.commands.answers;

public record class AddAnswerToGeneralTestQuestionCommand(
    GeneralTestQuestionId QuestionId,
    GeneralTestAnswerTypeSpecificData AnswerData,
    HashSet<GeneralTestResultId> RelatedResultIds
) : IRequest<ErrOrNothing>;

internal class AddAnswerToGeneralTestQuestionCommandHandler : IRequestHandler<AddAnswerToGeneralTestQuestionCommand, ErrOrNothing>
{
    private readonly IGeneralTestQuestionsRepository _questionsRepository;

    public AddAnswerToGeneralTestQuestionCommandHandler(IGeneralTestQuestionsRepository questionsRepository) {
        _questionsRepository = questionsRepository;
    }

    public async Task<ErrOrNothing> Handle(AddAnswerToGeneralTestQuestionCommand request, CancellationToken cancellationToken) {
        GeneralTestQuestion? question = await _questionsRepository.GetWithAnswers(request.QuestionId);
        if (question is null) {
            return Err.ErrPresets.GeneralTestQuestionNotFound(request.QuestionId);
        }

        var addingRes = question.AddNewAnswer(request.AnswerData, request.RelatedResultIds);
        if (addingRes.IsErr(out var err)) { return err; }

        await _questionsRepository.Update(question);
        return ErrOrNothing.Nothing;
    }
}
