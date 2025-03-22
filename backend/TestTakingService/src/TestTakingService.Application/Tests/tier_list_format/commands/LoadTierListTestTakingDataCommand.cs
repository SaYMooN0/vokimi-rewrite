using MediatR;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using TestTakingService.Application.Common.interfaces.repositories.tests;
using TestTakingService.Domain.TestAggregate.tier_list_format;

namespace TestTakingService.Application.Tests.tier_list_format.commands;

public record class LoadTierListTestTakingDataCommand(TestId TestId) : IRequest<ErrOr<TierListFormatTest>>;

public class LoadTierListTestTakingDataCommandHandler
    : IRequestHandler<LoadTierListTestTakingDataCommand, ErrOr<TierListFormatTest>>
{
    private ITierListFormatTestsRepository _tierListFormatRepository;

    public LoadTierListTestTakingDataCommandHandler(ITierListFormatTestsRepository tierListFormatRepository) {
        _tierListFormatRepository = tierListFormatRepository;
    }

    public async Task<ErrOr<TierListFormatTest>> Handle(
        LoadTierListTestTakingDataCommand request,
        CancellationToken cancellationToken
    ) {
        TierListFormatTest? test = await _tierListFormatRepository.GetWithItemsAndTiers(request.TestId);
        if (test is null) {
            return Err.ErrPresets.TierListTestNotFound(request.TestId);
        }

        return ErrOr<TierListFormatTest>.Success(test);
    }
}