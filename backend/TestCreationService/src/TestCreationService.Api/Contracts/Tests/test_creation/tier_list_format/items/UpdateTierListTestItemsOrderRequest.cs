using ApiShared.interfaces;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using TestCreationService.Domain.Common;

namespace TestCreationService.Api.Contracts.Tests.test_creation.tier_list_format.items;

internal class UpdateTierListTestItemsOrderRequest : IRequestWithValidationNeeded
{
    public bool ShuffleAnswers { get; init; }
    public Dictionary<string, int> ItemsOrder { get; init; } = [];

    public RequestValidationResult Validate() {
        if (CreateOrderController().IsErr(out var err)) {
            return err;
        }

        return RequestValidationResult.Success;
    }

    public ErrOr<EntitiesOrderController<TierListTestItemId>> CreateOrderController() {
        if (ItemsOrder.Keys.Any(id => !Guid.TryParse(id, out var _))) {
            return Err.ErrFactory.InvalidData("Items order values have invalid id format");
        }

        if (ItemsOrder.Values.Any(order => order < 0 || order > ushort.MaxValue)) {
            return Err.ErrFactory.InvalidData("Items order values have invalid order value");
        }

        return EntitiesOrderController<TierListTestItemId>.CreateNew(
            ShuffleAnswers,
            ItemsOrder.ToDictionary(
                kvp => new TierListTestItemId(new Guid(kvp.Key)),
                kvp => (ushort)kvp.Value
            )
        );
    }
}