using MediatR;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.Common;
using TestCreationService.Domain.TestAggregate.tier_list_format;

namespace TestCreationService.Application.Tests.tier_list_format.commands.items;

public record class UpdateTierListTestItemsOrderCommand(
    TestId TestId,
    EntitiesOrderController<TierListTestItemId> OrderController
) : IRequest<ErrOrNothing>;

internal class UpdateTierListTestItemsOrderCommandHandler :
    IRequestHandler<UpdateTierListTestItemsOrderCommand, ErrOrNothing>
{
    private readonly ITierListFormatTestsRepository _TierListFormatTestsRepository;

    public UpdateTierListTestItemsOrderCommandHandler(
        ITierListFormatTestsRepository TierListFormatTestsRepository
    ) {
        _TierListFormatTestsRepository = TierListFormatTestsRepository;
    }

    public async Task<ErrOrNothing> Handle(
        UpdateTierListTestItemsOrderCommand request, CancellationToken cancellationToken
    ) {
        TierListFormatTest? test = await _TierListFormatTestsRepository.GetById(request.TestId);
        if (test is null) {
            return Err.ErrPresets.TierListTestNotFound(request.TestId);
        }

        var updateErr = test.UpdateItemsOrder(request.OrderController);
        if (updateErr.IsErr(out var err)) {
            return err;
        }

        await _TierListFormatTestsRepository.Update(test);
        return ErrOrNothing.Nothing;
    }
}