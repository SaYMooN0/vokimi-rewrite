using SharedKernel.Common.domain.entity;
using TestManagingService.Domain.TestAggregate;

namespace TestManagingService.Application.Common.interfaces.repositories.tests;

public interface IBaseTestsRepository
{
    public Task<BaseTest?> GetById(TestId testId);
    public Task<BaseTest?> GetWithTagSuggestions(TestId testId);
    public Task<BaseTest?> GetWithCommentReports(TestId testId);
    public Task Update(BaseTest test);

}