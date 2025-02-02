using SharedKernel.Common.domain;

namespace TestCreationService.Domain.TestAggregate.general_format.events;
public record class RelatedResultsForGeneralTestAnswerChangedEvent(
    TestId TestId,
    HashSet<GeneralTestResultId> RelatedResults
) : IDomainEvent;