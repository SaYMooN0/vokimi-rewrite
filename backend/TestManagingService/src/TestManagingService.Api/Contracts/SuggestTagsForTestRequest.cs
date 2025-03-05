using ApiShared.interfaces;
using SharedKernel.Common;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;

namespace TestManagingService.Api.Contracts;

public class SuggestTagsForTestRequest : IRequestWithValidationNeeded
{
    public string[] SuggestedTags { get; set; } = [];

    public RequestValidationResult Validate() {
        if (SuggestedTags.Length == 0) {
            return Err.ErrFactory.InvalidData("No suggested tags were specified");
        }

        if (SuggestedTags.Length > 200) {
            return Err.ErrFactory.InvalidData(
                "Too many tags were specified. Please specify less tags"
            );
        }

        ErrList errs = new();
        foreach (var tag in SuggestedTags) {
            if (!TestTagsRules.IsStringValidTag(tag)) {
                errs.Add(Err.ErrFactory.InvalidData(
                    message: $"'{tag}' is not a valid tag"
                ));
            }
        }

        return errs;
    }

    public HashSet<TestTagId> GetParsedTags() =>
        SuggestedTags.Select(x => new TestTagId(x)).ToHashSet();
}