using ApiShared.interfaces;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests.tier_list_format;

namespace TestCreationService.Api.Contracts.Tests.test_creation.tier_list_format.items;

internal class SaveNewItemForTierListTestRequest : IRequestWithValidationNeeded
{
    public string ItemName { get; init; }
    public string? ItemClarification { get; init; } = null;
    public TierListTestItemContentType ItemContentType { get; init; }
    public Dictionary<string, string> ContentTypeSpecificData { get; init; } = [];

    public RequestValidationResult Validate() {
        if ()
    }

    public ErrOr<TierListTestItemContentData> ParsedItemContentData() =>
        TierListTestItemContentData.CreateFromDictionary(ItemContentType, ContentTypeSpecificData);
}