using MediatR;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.Common;
using TestCreationService.Domain.TestAggregate.tier_list_format;

namespace TestCreationService.Application.Tests.tier_list_format.commands.tiers;

public record class UpdateTierListTestTiersOrderCommand(
    TestId TestId,
    EntitiesOrderController<TierListTestTierId> OrderController
) : IRequest<ErrOrNothing>;

internal class UpdateTierListTestTiersOrderCommandHandler :
    IRequestHandler<UpdateTierListTestTiersOrderCommand, ErrOrNothing>
{
    private readonly ITierListFormatTestsRepository _TierListFormatTestsRepository;

    public UpdateTierListTestTiersOrderCommandHandler(
        ITierListFormatTestsRepository TierListFormatTestsRepository
    ) {
        _TierListFormatTestsRepository = TierListFormatTestsRepository;
    }

    public async Task<ErrOrNothing> Handle(
        UpdateTierListTestTiersOrderCommand request, CancellationToken cancellationToken
    ) {
        TierListFormatTest? test = await _TierListFormatTestsRepository.GetById(request.TestId);
        if (test is null) {
            return Err.ErrPresets.TierListTestNotFound(request.TestId);
        }

        var updateErr = test.UpdateTiersOrder(request.OrderController);
        if (updateErr.IsErr(out var err)) {
            return err;
        }

        await _TierListFormatTestsRepository.Update(test);
        return ErrOrNothing.Nothing;
    }
}