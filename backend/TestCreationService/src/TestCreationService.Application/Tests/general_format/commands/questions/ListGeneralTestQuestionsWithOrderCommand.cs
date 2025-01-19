using MediatR;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using System.Collections.Immutable;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.TestAggregate.general_format;

namespace TestCreationService.Application.Tests.general_format.commands.questions;

public record class ListGeneralTestQuestionsWithOrderCommand(
    TestId TestId
) : IRequest<ErrOr<IImmutableDictionary<ushort, GeneralTestQuestion>>>;
public class ListGeneralTestQuestionsCommandHandler
    : IRequestHandler<
        ListGeneralTestQuestionsWithOrderCommand,
        ErrOr<IImmutableDictionary<ushort, GeneralTestQuestion>>
    >
{
    private readonly IGeneralFormatTestsRepository generalFormatTestsRepository;

    public ListGeneralTestQuestionsCommandHandler(IGeneralFormatTestsRepository generalFormatTestsRepository) {
        this.generalFormatTestsRepository = generalFormatTestsRepository;
    }

    public async Task<ErrOr<IImmutableDictionary<ushort, GeneralTestQuestion>>> Handle(ListGeneralTestQuestionsWithOrderCommand request, CancellationToken cancellationToken) {
        GeneralFormatTest? test = await generalFormatTestsRepository.GetWithQuestions(request.TestId);
        if (test is null) {
            return Err.ErrFactory.NotFound(
                "Unable to find this general format test",
                details: $"Cannot find general format test with id {request.TestId}"
            );
        }
        return ErrOr<IImmutableDictionary<ushort, GeneralTestQuestion>>.Success(
            test.GetAllQuestionsWithCorrectOrder()
        );
    }
}
