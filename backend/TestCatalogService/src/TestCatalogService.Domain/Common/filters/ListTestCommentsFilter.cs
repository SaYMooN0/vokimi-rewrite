using TestCatalogService.Application.Common.sort_options;

namespace TestCatalogService.Application.Common.filters;

public record class ListTestCommentsFilter(
    uint? MinAnswersCount,
    uint? MaxAnswersCount,
    int? MinVotesRating,
    int? MaxVotesRating,
    uint? MinVotesCount,
    uint? MaxVotesCount,
    DateTime? DateFrom,
    DateTime? DateTo,
    bool ShowHidden,
    bool ShowDeleted,
    FilterTriState WithAttachments,
    
    //auth requiring
    // FilterTriState ByUser,
    // FilterTriState ByUserFollowings,
    // FilterTriState ByUserFollowers,

    TestCommentsSortOption Sorting
);