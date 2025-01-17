using TestCreationService.Domain.TestAggregate.scoring_format;

namespace TestCreationService.Application.Common.interfaces.repositories;

public interface IScoringFormatTestsRepository
{
    public Task AddNew(ScoringFormatTest test);

}
