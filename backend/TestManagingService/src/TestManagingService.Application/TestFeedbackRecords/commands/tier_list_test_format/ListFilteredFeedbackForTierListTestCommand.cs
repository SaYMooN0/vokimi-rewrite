using MediatR;
using SharedKernel.Common.domain.entity;
using TestManagingService.Application.Common.interfaces.repositories.feedback_records;
using TestManagingService.Domain.FeedbackRecordAggregate.filters;
using TestManagingService.Domain.FeedbackRecordAggregate.tier_list_test;

namespace TestManagingService.Application.TestFeedbackRecords.commands.tier_list_test_format;

public record ListFilteredFeedbackForTierListTestCommand(
    TestId TestId,
    TierListTestFeedbackRecordsQueryFilter QueryFilter
) : IRequest<TierListTestFeedbackRecord[]>;

internal class ListFilteredFeedbackForTierListTestCommandHandler
    : IRequestHandler<ListFilteredFeedbackForTierListTestCommand, TierListTestFeedbackRecord[]>
{
    private readonly ITierListTestFeedbackRecordsRepository _tierListTestFeedbackRecordsRepository;

    public ListFilteredFeedbackForTierListTestCommandHandler(
        ITierListTestFeedbackRecordsRepository tierListTestFeedbackRecordsRepository
    ) {
        _tierListTestFeedbackRecordsRepository = tierListTestFeedbackRecordsRepository;
    }


    public Task<TierListTestFeedbackRecord[]> Handle(
        ListFilteredFeedbackForTierListTestCommand request,
        CancellationToken cancellationToken
    ) => _tierListTestFeedbackRecordsRepository.ListFilteredForTestAsNoTracking(request.TestId, request.QueryFilter);
}