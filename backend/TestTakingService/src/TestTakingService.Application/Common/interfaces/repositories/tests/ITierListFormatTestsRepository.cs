using SharedKernel.Common.domain.entity;
using TestTakingService.Domain.TestAggregate.tier_list_format;

namespace TestTakingService.Application.Common.interfaces.repositories.tests;

public interface ITierListFormatTestsRepository
{
    public Task Add(TierListFormatTest test);
    public Task Update(TierListFormatTest test);
    public Task<TierListFormatTest?> GetById(TestId testId);
    public Task<TierListFormatTest?> GetWithItemsAndTiers(TestId testId);
}