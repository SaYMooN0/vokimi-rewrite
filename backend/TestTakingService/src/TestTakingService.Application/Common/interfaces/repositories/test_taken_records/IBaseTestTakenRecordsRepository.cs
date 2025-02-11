using TestTakingService.Domain.TestTakenRecordAggregate;

namespace TestTakingService.Application.Common.interfaces.repositories.test_taken_records;

public interface IBaseTestTakenRecordsRepository
{
    public Task Add(BaseTestTakenRecord testTakenRecord);

}