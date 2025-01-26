using MediatR;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.GeneralTestQuestionAggregate;

namespace TestCreationService.Application.GeneralTestQuestions.commands.answers;


public record class RemoveAnswerFromGeneralTestQuestionCommand(
    GeneralTestQuestionId QuestionId,
    GeneralTestAnswerId AnswerId
) : IRequest<ErrOrNothing>;
internal class RemoveAnswerFromGeneralTestQuestionCommandHandler : IRequestHandler<RemoveAnswerFromGeneralTestQuestionCommand, ErrOrNothing>
{
    private readonly IGeneralTestQuestionsRepository _questionsRepository;

    public RemoveAnswerFromGeneralTestQuestionCommandHandler(IGeneralTestQuestionsRepository questionsRepository) {
        _questionsRepository = questionsRepository;
    }

    public async Task<ErrOrNothing> Handle(RemoveAnswerFromGeneralTestQuestionCommand request, CancellationToken cancellationToken) {
        GeneralTestQuestion? question = await _questionsRepository.GetWithAnswers(request.QuestionId);
        if (question is null) {
            return Err.ErrPresets.GeneralTestQuestionNotFound(request.QuestionId);
        }

        var removeRes = question.RemoveAnswer(request.AnswerId);
        if (removeRes.IsErr(out var err)) {
            return err;
        }

        await _questionsRepository.Update(question);
        return ErrOrNothing.Nothing;
    }
}
