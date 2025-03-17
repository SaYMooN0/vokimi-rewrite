using MediatR;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.TestAggregate.tier_list_format;

namespace TestCreationService.Application.Tests.tier_list_format.commands.items;

public record RemoveTierListTestItemCommand(
    TestId TestId,
    TierListTestItemId ItemId
) : IRequest<ErrOrNothing>;

public class RemoveTierListTestItemCommandHandler
    : IRequestHandler<RemoveTierListTestItemCommand, ErrOrNothing>
{
    private readonly ITierListFormatTestsRepository _tierListFormatRepository;

    public RemoveTierListTestItemCommandHandler(
        ITierListFormatTestsRepository tierListFormatRepository
    ) {
        _tierListFormatRepository = tierListFormatRepository;
    }

    public async Task<ErrOrNothing> Handle(
        RemoveTierListTestItemCommand request, CancellationToken cancellationToken
    ) {
        TierListFormatTest? test = await _tierListFormatRepository.GetWithItemsIncluded(request.TestId);
        if (test is null) {
            return Err.ErrPresets.TierListTestNotFound(request.TestId);
        }

        var removeRes = test.RemoveItem(request.ItemId);
        if (removeRes.IsErr(out var err)) {
            return err;
        }

        await _tierListFormatRepository.Update(test);
        return ErrOrNothing.Nothing;
    }
}