using ApiShared.interfaces;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests.tier_list_format.items;

namespace TestCreationService.Api.Contracts.Tests.test_creation.tier_list_format.items;

internal class SaveItemForTierListTestRequest : IRequestWithValidationNeeded
{
    public string ItemName { get; init; }
    public string? ItemClarification { get; init; } = null;
    public TierListTestItemContentType? ItemContentType { get; init; } = null;
    public Dictionary<string, string> ContentTypeSpecificData { get; init; } = [];

    public RequestValidationResult Validate() {
        if (ItemContentType is null) {
            return Err.ErrFactory.InvalidData("Item content type is not proveided");
        }

        if (
            TierListTestItemRules.CheckIfStringCorrectItemName(ItemName).IsErr(out var err)
            || TierListTestItemRules.CheckIfStringCorrectItemClarification(ItemClarification)
                .IsErr(out err)
            || ParsedItemContentData().IsErr(out err)
        ) {
            return err;
        }

        return RequestValidationResult.Success;
    }

    public ErrOr<TierListTestItemContentData> ParsedItemContentData() =>
        TierListTestItemContentData.CreateFromDictionary(ItemContentType.Value, ContentTypeSpecificData);
}