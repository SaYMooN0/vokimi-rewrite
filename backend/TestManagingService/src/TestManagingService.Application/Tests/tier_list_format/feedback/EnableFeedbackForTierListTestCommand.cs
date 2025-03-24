using MediatR;
using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests.tier_list_format.feedback;
using TestManagingService.Application.Common.interfaces.repositories.tests;
using TestManagingService.Domain.TestAggregate.tier_list_format;

namespace TestManagingService.Application.Tests.tier_list_format.feedback;

public record EnableFeedbackForTierListTestCommand(
    TestId TestId,
    AnonymityValues Anonymity,
    string AccompanyingText,
    ushort MaxFeedbackLength
) : IRequest<ErrOr<TierListTestFeedbackOption>>;

internal class EnableFeedbackForTierListTestCommandHandler
    : IRequestHandler<EnableFeedbackForTierListTestCommand, ErrOr<TierListTestFeedbackOption>>
{
    private readonly ITierListFormatTestsRepository _tierListFormatTestsRepository;

    public EnableFeedbackForTierListTestCommandHandler(
        ITierListFormatTestsRepository tierListFormatTestsRepository
    ) {
        _tierListFormatTestsRepository = tierListFormatTestsRepository;
    }

    public async Task<ErrOr<TierListTestFeedbackOption>> Handle(
        EnableFeedbackForTierListTestCommand request,
        CancellationToken cancellationToken
    ) {
        TierListFormatTest? test = await _tierListFormatTestsRepository.GetById(request.TestId);
        if (test is null) {
            return Err.ErrPresets.TestNotFound(request.TestId);
        }

        var result = test.SetFeedbackOptionEnabled(
            request.Anonymity,
            request.AccompanyingText,
            request.MaxFeedbackLength
        );
        if (result.IsErr(out var err)) {
            return err;
        }

        await _tierListFormatTestsRepository.Update(test);
        return test.FeedbackOption;
    }
}