using SharedKernel.Common.EntityIds;
using SharedKernel.Common;

namespace TestCreationService.Domain.TestAggregate.general_format.events;
public record class RelatedResultsForGeneralTestAnswerChangedEvent(
    GeneralTestAnswerId AnswerId,
    HashSet<GeneralTestResultId> RelatedResults
) : IDomainEvent;