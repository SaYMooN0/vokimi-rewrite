using TestCatalogService.Domain.Common;

namespace TestCatalogService.Application.Common.filters;

public record class ListTestCommentsFilter(
    //check parent comment
    ushort? MinAnswersCount,
    ushort? MaxAnswersCount,
    ushort? MinVotesRating,
    ushort? MaxVotesRating,
    int? MinVotesCount,
    int? MaxVotesCount,
    DateTime? DateFrom,
    DateTime? DateTo,
    FilterTriState WithAttachments,
    FilterTriState ByUserFollowings,
    FilterTriState ByUserFollowers
) { }