using MediatR;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests.tier_list_format;
using SharedKernel.Common.tests.tier_list_format.items;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.TestAggregate.tier_list_format;

namespace TestCreationService.Application.Tests.tier_list_format.commands.items;

public record class SaveNewItemForTierListTestCommand(
    TestId TestId,
    string ItemName,
    string? ItemClarification,
    TierListTestItemContentData ItemContent
) : IRequest<ErrOr<TierListTestItem>>;

internal class SaveNewItemForTierListTestCommandHandler
    : IRequestHandler<SaveNewItemForTierListTestCommand, ErrOr<TierListTestItem>>
{
    private readonly ITierListFormatTestsRepository _tierListFormatRepository;

    public SaveNewItemForTierListTestCommandHandler(
        ITierListFormatTestsRepository tierListFormatRepository
    ) {
        _tierListFormatRepository = tierListFormatRepository;
    }

    public async Task<ErrOr<TierListTestItem>> Handle(
        SaveNewItemForTierListTestCommand request, CancellationToken cancellationToken
    ) {
        TierListFormatTest? test = await _tierListFormatRepository.GetWithItemsIncluded(request.TestId);
        if (test is null) {
            return Err.ErrPresets.TierListTestNotFound(request.TestId);
        }

        ErrOr<TierListTestItem> addingRes = test.AddNewItem(
            request.ItemName, request.ItemClarification, request.ItemContent
        );
        if (addingRes.IsErr(out var err)) {
            return err;
        }

        await _tierListFormatRepository.Update(test);
        return addingRes.GetSuccess();
    }
}