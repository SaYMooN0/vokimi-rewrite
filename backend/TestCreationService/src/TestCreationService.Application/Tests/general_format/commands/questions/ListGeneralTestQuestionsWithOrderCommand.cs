using MediatR;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using System.Collections.Immutable;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.TestAggregate.general_format;

namespace TestCreationService.Application.Tests.general_format.commands.questions;

public record class ListGeneralTestQuestionsWithOrderCommand(
    TestId TestId
) : IRequest<ErrOr<IReadOnlyList<(GeneralTestQuestion Question, ushort Order)>>>;
public class ListGeneralTestQuestionsCommandHandler
    : IRequestHandler<
        ListGeneralTestQuestionsWithOrderCommand,
        ErrOr<IReadOnlyList<(GeneralTestQuestion, ushort)>>
    >
{
    private readonly IGeneralFormatTestsRepository generalFormatTestsRepository;

    public ListGeneralTestQuestionsCommandHandler(IGeneralFormatTestsRepository generalFormatTestsRepository) {
        this.generalFormatTestsRepository = generalFormatTestsRepository;
    }

    public async Task<ErrOr<IReadOnlyList<(GeneralTestQuestion, ushort)>>> Handle(ListGeneralTestQuestionsWithOrderCommand request, CancellationToken cancellationToken) {
        GeneralFormatTest? test = await generalFormatTestsRepository.GetWithQuestions(request.TestId);
        if (test is null) {
            return Err.ErrPresets.GeneralTestNotFound(request.TestId);
        }
        return ErrOr<IReadOnlyList<(GeneralTestQuestion, ushort)>>.Success(test.GetQuestionsWithOrder());
    }
}
