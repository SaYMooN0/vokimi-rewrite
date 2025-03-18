using MediatR;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests.tier_list_format;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.TestAggregate.tier_list_format;

namespace TestCreationService.Application.Tests.tier_list_format.commands.tiers;
public record UpdateTierListTestTierCommand(
    TestId TestId,
    TierListTestTierId TierId,
    string TierName,
    string? TierDescription,
    ushort? MaxItemsCountLimit,
    TierListTestTierStyles Styles
) : IRequest<ErrOr<TierListTestTier>>;

internal class UpdateTierListTestTierCommandHandler
    : IRequestHandler<UpdateTierListTestTierCommand, ErrOr<TierListTestTier>>
{
    private readonly ITierListFormatTestsRepository _tierListFormatRepository;

    public UpdateTierListTestTierCommandHandler(
        ITierListFormatTestsRepository tierListFormatRepository
    ) {
        _tierListFormatRepository = tierListFormatRepository;
    }

    public async Task<ErrOr<TierListTestTier>> Handle(
        UpdateTierListTestTierCommand request, CancellationToken cancellationToken
    ) {
        TierListFormatTest? test = await _tierListFormatRepository.GetWithTiersIncluded(request.TestId);
        if (test is null) {
            return Err.ErrPresets.TierListTestNotFound(request.TestId);
        }

        ErrOr<TierListTestTier> updateRes = test.UpdateTier(
            request.TierId, request.TierName, request.TierDescription, request.MaxItemsCountLimit, request.Styles
        );
        if (updateRes.IsErr(out var err)) {
            return err;
        }

        await _tierListFormatRepository.Update(test);
        return updateRes.GetSuccess();
    }
}