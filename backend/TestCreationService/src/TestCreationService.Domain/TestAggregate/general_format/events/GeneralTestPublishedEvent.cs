using SharedKernel.Common;
using SharedKernel.Common.common_enums;
using SharedKernel.Common.EntityIds;
using SharedKernel.IntegrationEvents.test_publishing;

namespace TestCreationService.Domain.TestAggregate.general_format.events;

public record class GeneralTestPublishedEvent(
    TestId TestId,
    AppUserId CreatorId,
    AppUserId[] EditorIds,
    string Name,
    string CoverImage,
    string Description,
    Language Language,
    TestPublishedInteractionsAccessSettingsDto InteractionsAccessSettings,
    TestPublishedStylesDto Styles,
    string[] Tags,
    TestTakingProcessSettings TestTakingProcessSettings,
    bool ShuffleQuestions,
    GeneralTestPublishedQuestionDto[] Questions,
    GeneralTestPublishedResultDto[] Results
) : IDomainEvent
{
}
