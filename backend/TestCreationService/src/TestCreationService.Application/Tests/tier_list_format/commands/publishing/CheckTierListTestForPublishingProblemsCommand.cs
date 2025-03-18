using MediatR;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.TestAggregate.formats_shared;
using TestCreationService.Domain.TestAggregate.tier_list_format;

namespace TestCreationService.Application.Tests.tier_list_format.commands.publishing;

public record class CheckTierListTestForPublishingProblemsCommand(
    TestId TestId
) : IRequest<ErrOr<TestPublishingProblem[]>>;

public class CheckTierListTestForPublishingProblemsCommandHandler :
    IRequestHandler<CheckTierListTestForPublishingProblemsCommand, ErrOr<TestPublishingProblem[]>>
{
    private readonly ITierListFormatTestsRepository _tierListFormatTestsRepository;

    public CheckTierListTestForPublishingProblemsCommandHandler(
        ITierListFormatTestsRepository tierListFormatTestsRepository
    ) {
        _tierListFormatTestsRepository = tierListFormatTestsRepository;
    }

    public async Task<ErrOr<TestPublishingProblem[]>> Handle(
        CheckTierListTestForPublishingProblemsCommand request,
        CancellationToken cancellationToken
    ) {
        TierListFormatTest? test = await _tierListFormatTestsRepository.GetWithEverything(request.TestId);
        if (test is null) {
            return Err.ErrPresets.TierListTestNotFound(request.TestId);
        }

        return test.CheckForPublishingProblems();
    }
}