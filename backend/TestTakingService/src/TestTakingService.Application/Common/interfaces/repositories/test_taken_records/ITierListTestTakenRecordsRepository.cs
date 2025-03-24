using TestTakingService.Domain.TestTakenRecordAggregate.tier_list_test;

namespace TestTakingService.Application.Common.interfaces.repositories.test_taken_records;

public interface ITierListTestTakenRecordsRepository
{
    public Task Add(TierListTestTakenRecord tierListTestTakenRecord);

}