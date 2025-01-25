using MediatR;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests.general_format_tests.answer_type_specific_data;
using TestCreationService.Application.Common.interfaces.repositories.general_format_tests;
using TestCreationService.Domain.TestAggregate.general_format;

namespace TestCreationService.Application.Tests.general_format.commands.answers;
public record class AddAnswerToGeneralTestQuestionCommand(
    GeneralTestQuestionId QuestionId,
    GeneralTestAnswerTypeSpecificData AnswerData
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

        var addingRes = question.AddNewAnswer(request.AnswerData);
        if (addingRes.IsErr(out var err)) { return err; }

        await _questionsRepository.Update(question);
        return ErrOrNothing.Nothing;
    }
}
