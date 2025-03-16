using SharedKernel.Common.domain.entity;
using TestCreationService.Domain.TestAggregate.tier_list_format;

namespace TestCreationService.Application.Common.interfaces.repositories;

public interface ITierListFormatTestsRepository
{
    public Task AddNew(TierListFormatTest test);
    public Task<TierListFormatTest?> GetById(TestId testId);
    public Task Update(TierListFormatTest test);
    public Task<TierListFormatTest?> GetWithItemsIncluded(TestId testId);
    public Task<TierListFormatTest?> GetWithTiersIncluded(TestId testId);
}