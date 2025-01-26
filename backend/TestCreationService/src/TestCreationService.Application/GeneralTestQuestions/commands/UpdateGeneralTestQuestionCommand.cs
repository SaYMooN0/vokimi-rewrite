using MediatR;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using SharedKernel.Common.general_test_questions;
using SharedKernel.Common.general_test_questions.answer_type_specific_data;

namespace TestCreationService.Application.GeneralTestQuestions.commands;


public record class UpdateGeneralTestQuestionCommand(
    GeneralTestQuestionId QuestionId,
    string Text,
    string[] Images,
    GeneralTestQuestionTimeLimitOption TimeLimit,
    GeneralTestQuestionAnswersCountLimit AnswerCountLimit
) : IRequest<ErrOrNothing>;
internal class UpdateGeneralTestQuestionCommandHandler : IRequestHandler<UpdateGeneralTestQuestionCommand, ErrOrNothing>
{
    public async Task<ErrOrNothing> Handle(UpdateGeneralTestQuestionCommand request, CancellationToken cancellationToken) {
    }
}
