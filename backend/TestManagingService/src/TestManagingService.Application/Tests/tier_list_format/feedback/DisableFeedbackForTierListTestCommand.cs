using MediatR;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests.tier_list_format.feedback;
using TestManagingService.Application.Common.interfaces.repositories.tests;
using TestManagingService.Domain.TestAggregate.tier_list_format;

namespace TestManagingService.Application.Tests.tier_list_format.feedback;

public record DisableFeedbackForTierListTestCommand(
    TestId TestId
) : IRequest<ErrOr<TierListTestFeedbackOption>>;

internal class DisableFeedbackForTierListTestCommandHandler
    : IRequestHandler<DisableFeedbackForTierListTestCommand, ErrOr<TierListTestFeedbackOption>>
{
    private readonly ITierListFormatTestsRepository _tierListFormatTestsRepository;

    public DisableFeedbackForTierListTestCommandHandler(
        ITierListFormatTestsRepository tierListFormatTestsRepository
    ) {
        _tierListFormatTestsRepository = tierListFormatTestsRepository;
    }

    public async Task<ErrOr<TierListTestFeedbackOption>> Handle(
        DisableFeedbackForTierListTestCommand request,
        CancellationToken cancellationToken
    ) {
        TierListFormatTest? test = await _tierListFormatTestsRepository.GetById(request.TestId);
        if (test is null) {
            return Err.ErrPresets.TestNotFound(request.TestId);
        }

        test.SetFeedbackOptionDisabled();

        await _tierListFormatTestsRepository.Update(test);
        return test.FeedbackOption;
    }
}