using TestManagingService.Domain.TestAggregate.formats_shared;

namespace TestManagingService.Api.Contracts;

public record class TestTagSuggestionViewResponse(
    string Tag,
    DateTime FirstTimeSuggested,
    uint Count
)
{
    public static TestTagSuggestionViewResponse FromSuggestion(TagSuggestionForTest tagSuggestion) => new(
        tagSuggestion.Tag.Value,
        tagSuggestion.FirstTimeSuggested,
        tagSuggestion.Count
    );
}