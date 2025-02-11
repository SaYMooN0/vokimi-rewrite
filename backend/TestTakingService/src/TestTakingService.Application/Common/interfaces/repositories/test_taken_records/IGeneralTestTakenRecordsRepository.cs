using TestTakingService.Domain.TestTakenRecordAggregate.general_test;

namespace TestTakingService.Application.Common.interfaces.repositories.test_taken_records;

public interface IGeneralTestTakenRecordsRepository
{
    public Task Add(GeneralTestTakenRecord generalTestTakenRecord);
}