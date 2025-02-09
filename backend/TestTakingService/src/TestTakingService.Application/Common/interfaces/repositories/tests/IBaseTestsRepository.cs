using SharedKernel.Common.domain;
using TestTakingService.Domain.TestAggregate;

namespace TestTakingService.Application.Common.interfaces.repositories.tests;

public interface IBaseTestsRepository
{
    public Task<BaseTest?> GetById(TestId testId);
}