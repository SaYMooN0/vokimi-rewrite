using MediatR;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;

namespace TestCreationService.Application.GeneralTestQuestions.commands;

public record class RemoveGeneralTestQuestionCommand(
    GeneralTestQuestionId QuestionId
) : IRequest<ErrOrNothing>;
internal class RemoveGeneralTestQuestionCommandHandler : IRequestHandler<RemoveGeneralTestQuestionCommand, ErrOrNothing>
{
    public async Task<ErrOrNothing> Handle(RemoveGeneralTestQuestionCommand request, CancellationToken cancellationToken) {

    }
}
