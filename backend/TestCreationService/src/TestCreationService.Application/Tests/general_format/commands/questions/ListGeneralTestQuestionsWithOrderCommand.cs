using System.Collections.Immutable;
using MediatR;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.GeneralTestQuestionAggregate;
using TestCreationService.Domain.TestAggregate.general_format;

namespace TestCreationService.Application.Tests.general_format.commands.questions;

public record class ListGeneralTestQuestionsWithOrderCommand(
    TestId TestId
) : IRequest<ErrOr<ImmutableArray<(GeneralTestQuestion Question, ushort Order)>>>;
internal class ListGeneralTestQuestionsCommandHandler
    : IRequestHandler<
        ListGeneralTestQuestionsWithOrderCommand,
        ErrOr<ImmutableArray<(GeneralTestQuestion, ushort)>>
    >
{
    private readonly IGeneralFormatTestsRepository _generalFormatTestsRepository;
    private readonly IGeneralTestQuestionsRepository _generalTestQuestionsRepository;

    public ListGeneralTestQuestionsCommandHandler(IGeneralFormatTestsRepository generalFormatTestsRepository, IGeneralTestQuestionsRepository generalTestQuestionsRepository) {
        _generalFormatTestsRepository = generalFormatTestsRepository;
        _generalTestQuestionsRepository = generalTestQuestionsRepository;
    }

    public async Task<ErrOr<ImmutableArray<(GeneralTestQuestion, ushort)>>> Handle(ListGeneralTestQuestionsWithOrderCommand request, CancellationToken cancellationToken) {
        GeneralFormatTest? test = await _generalFormatTestsRepository.GetById(request.TestId);
        if (test is null) {
            return Err.ErrPresets.GeneralTestNotFound(request.TestId);
        }
        var questionsFetchRes = await _generalTestQuestionsRepository.GetAllWithId(test.GetTestQuestionIds());
        if (questionsFetchRes.IsErr(out var err)) {
            return err;
        }
        var questionsWithOrderRes = test.GetQuestionsWithOrder(questionsFetchRes.GetSuccess());
        if (questionsWithOrderRes.IsErr(out err)) {
            return err;
        }
        return ErrOr<ImmutableArray<(GeneralTestQuestion, ushort)>>.Success(
            questionsWithOrderRes.GetSuccess()
        );
    }
}
