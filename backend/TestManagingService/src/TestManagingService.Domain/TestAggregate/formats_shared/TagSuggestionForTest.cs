using SharedKernel.Common.domain.entity;
using SharedKernel.Common.interfaces;
using TestManagingService.Domain.Common;

namespace TestManagingService.Domain.TestAggregate.formats_shared;

public class TagSuggestionForTest : Entity<TagSuggestionForTestId>
{
    private TagSuggestionForTest() { }
    public TestTagId Tag { get; init; }
    public uint Count { get; private set; }
    public DateTime FirstTimeSuggested { get; init; }

    public static TagSuggestionForTest CreateNew(
        TestTagId tag, IDateTimeProvider dateTimeProvider
    ) => new() {
        Id = TagSuggestionForTestId.CreateNew(),
        Tag = tag,
        Count = 1,
        FirstTimeSuggested = dateTimeProvider.Now
    };
}