using MediatR;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.interfaces;
using TestTakingService.Application.Common.interfaces.repositories.tests;
using TestTakingService.Domain.Common.test_taken_data.tier_list_format_test;
using TestTakingService.Domain.TestAggregate.tier_list_format;


namespace TestTakingService.Application.Tests.tier_list_format.commands;

public record class TierListTestTakenCommand(
    TestId TestId,
    AppUserId? TestTakerId,
    Dictionary<TierListTestTierId, TierListTestTakenTierData> ItemsInTiers,
    TierListTestTakenFeedbackData? FeedbackData,
    DateTime TestTakingStart,
    DateTime TestTakingEnd
) : IRequest<ErrOr<Dictionary<TierListTestTierId, TierListTestTakenTierData>>>;

public class TierListTestTakenCommandHandler : IRequestHandler<
    TierListTestTakenCommand,
    ErrOr<Dictionary<TierListTestTierId, TierListTestTakenTierData>>
>
{
    private readonly ITierListFormatTestsRepository _tierListFormatRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public TierListTestTakenCommandHandler(
        ITierListFormatTestsRepository tierListFormatRepository,
        IDateTimeProvider dateTimeProvider
    ) {
        _tierListFormatRepository = tierListFormatRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ErrOr<Dictionary<TierListTestTierId, TierListTestTakenTierData>>> Handle(
        TierListTestTakenCommand request,
        CancellationToken cancellationToken
    ) {
        TierListFormatTest? test = await _tierListFormatRepository.GetWithItemsAndTiers(request.TestId);
        if (test is null) {
            return Err.ErrPresets.TierListTestNotFound(request.TestId);
        }

        var testTakenRes = test.TestTaken(
            request.TestTakerId,
            request.ItemsInTiers,
            testTakingStart: request.TestTakingStart,
            testTakingEnd: request.TestTakingEnd,
            request.FeedbackData,
            _dateTimeProvider
        );
        
        if (testTakenRes.IsErr(out var err)) {
            return err;
        }

        await _tierListFormatRepository.Update(test);
        return testTakenRes.GetSuccess();
    }
}