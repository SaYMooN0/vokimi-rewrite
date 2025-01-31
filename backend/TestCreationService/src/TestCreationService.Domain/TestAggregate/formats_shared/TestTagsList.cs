using SharedKernel.Common;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using System.Collections.Immutable;
using TestCreationService.Domain.Common;

namespace TestCreationService.Domain.TestAggregate.formats_shared;

public class TestTagsList : Entity<TestTagsListId>
{
    private TestTagsList() { }
    private TestId TestId { get; init; }
    private HashSet<string> _tags;
    public static TestTagsList CreateNew(TestId testId) => new() {
        TestId = testId,
        Id = TestTagsListId.CreateNew(),
        _tags = []
    };
    public ErrListOr<ISet<string>> Update(IEnumerable<string> tagsToSet) {
        ErrList errs = new();
        HashSet<string> newTags = tagsToSet.ToHashSet();
        if (newTags.Count > TestTagsRules.MaxTagsForTestCount) {
            errs.Add(Err.ErrFactory.InvalidData(
                $"Too many tags selected. Test cannot have more than {TestTagsRules.MaxTagsForTestCount} tags",
                details: $"Current count of unique tags: {newTags.Count}"
            ));
        }

        //to not check for too many tags
        if (newTags.Count > TestTagsRules.MaxTagsForTestCount * 2) { return errs; }

        foreach (var tag in newTags) {
            if (!TestTagsRules.IsStringValidTag(tag)) {
                errs.Add(Err.ErrFactory.InvalidData($"Invalid tag: {tag}"));
            }
        }
        if (errs.Any()) { return errs; }

        _tags = newTags;
        return GetTags();
    }
    public ImmutableHashSet<string> GetTags() => _tags.ToImmutableHashSet();
    public void Clear() { _tags.Clear(); }
    public IEnumerable<Err> CheckForPublishingProblems() {
        if (_tags.Count > TestTagsRules.MaxTagsForTestCount) {
            yield return Err.ErrFactory.InvalidData(
                $"Test has too many tags.Test cannot have more than {TestTagsRules.MaxTagsForTestCount} tags"
            );
        }
    }
}
