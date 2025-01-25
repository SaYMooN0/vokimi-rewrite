using MediatR;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests.general_format_tests;
using SharedKernel.Common.tests.general_format_tests.answer_type_specific_data;
using TestCreationService.Application.Common.interfaces.repositories.general_format_tests;
using TestCreationService.Domain.TestAggregate.general_format;

namespace TestCreationService.Application.Tests.general_format.commands.answers;

public record class UpdateAnswerForGeneralTestQuestionCommand(
    GeneralTestQuestionId QuestionId,
    GeneralTestAnswerId AnswerId,
    GeneralTestAnswerTypeSpecificData AnswerData
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

        var updatingRes = question.UpdateAnswer(request.AnswerId, request.AnswerData);
        if (updatingRes.IsErr(out var err)) {
            return err;
        }

        await _questionsRepository.Update(question);
        return ErrOrNothing.Nothing;
    }
}
