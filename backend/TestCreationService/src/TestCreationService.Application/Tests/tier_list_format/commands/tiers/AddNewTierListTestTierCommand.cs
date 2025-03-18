using MediatR;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.TestAggregate.tier_list_format;

namespace TestCreationService.Application.Tests.tier_list_format.commands.tiers;

public record class AddNewTierListTestTierCommand(
    TestId TestId,
    string TierName
) : IRequest<ErrOr<TierListTestTier>>;

internal class AddNewTierListTestTierCommandHandler
    : IRequestHandler<AddNewTierListTestTierCommand, ErrOr<TierListTestTier>>
{
    private readonly ITierListFormatTestsRepository _tierListFormatRepository;

    public AddNewTierListTestTierCommandHandler(
        ITierListFormatTestsRepository tierListFormatRepository
    ) {
        _tierListFormatRepository = tierListFormatRepository;
    }

    public async Task<ErrOr<TierListTestTier>> Handle(
        AddNewTierListTestTierCommand request, CancellationToken cancellationToken
    ) {
        TierListFormatTest? test = await _tierListFormatRepository.GetWithTiersIncluded(request.TestId);
        if (test is null) {
            return Err.ErrPresets.TierListTestNotFound(request.TestId);
        }

        ErrOr<TierListTestTier> addingRes = test.AddNewTier(request.TierName);
        if (addingRes.IsErr(out var err)) {
            return err;
        }

        await _tierListFormatRepository.Update(test);
        return addingRes.GetSuccess();
    }
}