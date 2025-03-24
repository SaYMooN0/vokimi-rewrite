using MediatR;
using SharedKernel.Common.domain.entity;
using TestManagingService.Application.Common.interfaces.repositories.feedback_records;
using TestManagingService.Domain.FeedbackRecordAggregate.tier_list_test;

namespace TestManagingService.Application.TestFeedbackRecords.commands.tier_list_test_format;

public record ListFeedbackForTierListTestCommand(
    TestId TestId
) : IRequest<TierListTestFeedbackRecord[]>;

internal class ListFeedbackForTierListTestCommandHandler
    : IRequestHandler<ListFeedbackForTierListTestCommand, TierListTestFeedbackRecord[]>
{
    private readonly ITierListTestFeedbackRecordsRepository _tierListTestFeedbackRecordsRepository;

    public ListFeedbackForTierListTestCommandHandler(
        ITierListTestFeedbackRecordsRepository tierListTestFeedbackRecordsRepository
    ) {
        _tierListTestFeedbackRecordsRepository = tierListTestFeedbackRecordsRepository;
    }


    public Task<TierListTestFeedbackRecord[]> Handle(
        ListFeedbackForTierListTestCommand request,
        CancellationToken cancellationToken
    ) => _tierListTestFeedbackRecordsRepository.ListForTestAsNoTracking(request.TestId);
}