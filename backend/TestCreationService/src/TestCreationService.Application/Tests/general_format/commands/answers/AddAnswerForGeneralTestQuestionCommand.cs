using MediatR;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using TestCreationService.Application.Common.interfaces.repositories.general_format_tests;
using TestCreationService.Domain.TestAggregate.general_format;

namespace TestCreationService.Application.Tests.general_format.commands.answers;

public record class AddAnswerForGeneralTestQuestionCommand(
    GeneralTestQuestionId QuestionId
) : IRequest<ErrOrNothing>;
internal class AddAnswerForGeneralTestQuestionCommandHandler : IRequestHandler<AddAnswerForGeneralTestQuestionCommand, ErrOrNothing>
{
    private readonly IGeneralTestQuestionsRepository _questionsRepository;

    public AddAnswerForGeneralTestQuestionCommandHandler(IGeneralTestQuestionsRepository questionsRepository) {
        _questionsRepository = questionsRepository;
    }

    public async Task<ErrOrNothing> Handle(AddAnswerForGeneralTestQuestionCommand request, CancellationToken cancellationToken) {
        GeneralTestQuestion? question = await _questionsRepository.GetWithAnswers(request.QuestionId);
        if (question is null) {
            return Err.ErrPresets.GeneralTestQuestionNotFound(request.QuestionId);
        }
        var addingRes = question.AddNewAnswer();
        if (addingRes.IsErr(out var err)) { return err; }
        await _questionsRepository.Update(question);
        return ErrOrNothing.Nothing;
    }
}
