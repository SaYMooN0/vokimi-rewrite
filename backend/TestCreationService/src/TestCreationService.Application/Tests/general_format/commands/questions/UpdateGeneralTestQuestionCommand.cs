using MediatR;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests.value_objects.answers_count_limit;
using SharedKernel.Common.tests.value_objects.time_limit_option;

namespace TestCreationService.Application.Tests.general_format.commands.questions;

public record class UpdateGeneralTestQuestionCommand(
    TestId TestId,
    GeneralTestQuestionId QuestionId,
    string Text,
    string[] Images,
    GeneralTestQuestionTimeLimitOption TimeLimit,
    GeneralTestQuestionAnswersCountLimit AnswerCountLimit
) : IRequest<ErrOrNothing>;
public class UpdateGeneralTestQuestionCommandHandler : IRequestHandler<UpdateGeneralTestQuestionCommand, ErrOrNothing>
{
    public async Task<ErrOrNothing> Handle(UpdateGeneralTestQuestionCommand request, CancellationToken cancellationToken) {
        return Err.ErrFactory.NotImplemented();
    }
}
