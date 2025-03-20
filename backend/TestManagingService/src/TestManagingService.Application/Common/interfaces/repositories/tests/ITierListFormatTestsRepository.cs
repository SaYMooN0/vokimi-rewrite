using SharedKernel.Common.domain.entity;
using TestManagingService.Domain.TestAggregate.tier_list_format;

namespace TestManagingService.Application.Common.interfaces.repositories.tests;

public interface ITierListFormatTestsRepository
{
    public Task<TierListFormatTest?> GetById(TestId testId);
    public Task Add(TierListFormatTest tierListTest);
    public Task Update(TierListFormatTest test);
}