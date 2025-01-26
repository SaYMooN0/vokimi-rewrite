
using MediatR;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.GeneralTestQuestionAggregate;

namespace TestCreationService.Application.GeneralTestQuestions.commands.answers;


public record class ListGeneralTestAnswersWithOrderCommand(
    GeneralTestQuestionId QuestionId
) : IRequest<ErrOr<IReadOnlyList<(GeneralTestAnswer Answer, ushort Order)>>>;

internal class ListGeneralTestAnswersWithOrderCommandHandler
    : IRequestHandler<
        ListGeneralTestAnswersWithOrderCommand,
        ErrOr<IReadOnlyList<(GeneralTestAnswer, ushort)>>
    >
{
    private readonly IGeneralTestQuestionsRepository _questionsRepository;

    public ListGeneralTestAnswersWithOrderCommandHandler(IGeneralTestQuestionsRepository questionsRepository) {
        _questionsRepository = questionsRepository;
    }

    public async Task<ErrOr<IReadOnlyList<(GeneralTestAnswer, ushort)>>> Handle(ListGeneralTestAnswersWithOrderCommand request, CancellationToken cancellationToken) {
        GeneralTestQuestion? question = await _questionsRepository.GetWithAnswers(request.QuestionId);
        if (question is null) {
            return Err.ErrPresets.GeneralTestQuestionNotFound(request.QuestionId);
        }
        return ErrOr<IReadOnlyList<(GeneralTestAnswer, ushort)>>.Success(question.GetAnswersWithOrder());
    }
}
