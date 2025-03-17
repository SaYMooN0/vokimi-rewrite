using MediatR;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.TestAggregate.tier_list_format;

namespace TestCreationService.Application.Tests.tier_list_format.commands.tiers;

public record RemoveTierListTestTierCommand(
    TestId TestId,
    TierListTestTierId ItemId
) : IRequest<ErrOrNothing>;

public class RemoveTierListTestTierCommandHandler
    : IRequestHandler<RemoveTierListTestTierCommand, ErrOrNothing>
{
    private readonly ITierListFormatTestsRepository _tierListFormatRepository;

    public RemoveTierListTestTierCommandHandler(
        ITierListFormatTestsRepository tierListFormatRepository
    ) {
        _tierListFormatRepository = tierListFormatRepository;
    }

    public async Task<ErrOrNothing> Handle(
        RemoveTierListTestTierCommand request, CancellationToken cancellationToken
    ) {
        TierListFormatTest? test = await _tierListFormatRepository.GetWithTiersIncluded(request.TestId);
        if (test is null) {
            return Err.ErrPresets.TierListTestNotFound(request.TestId);
        }

        var removeRes = test.RemoveTier(request.ItemId);
        if (removeRes.IsErr(out var err)) {
            return err;
        }

        await _tierListFormatRepository.Update(test);
        return ErrOrNothing.Nothing;
    }
}