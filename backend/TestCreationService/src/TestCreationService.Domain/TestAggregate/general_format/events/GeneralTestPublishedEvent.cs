using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity_id;
using SharedKernel.Common.tests.general_format;
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
    DateTime PublicationDate,
    TestPublishedInteractionsAccessSettingsDto InteractionsAccessSettings,
    TestPublishedStylesDto Styles,
    string[] Tags,
    GeneralTestFeedbackOption FeedbackOption,
    GeneralTestPublishedQuestionDto[] Questions,
    bool ShuffleQuestions,
    GeneralTestPublishedResultDto[] Results
) : IDomainEvent;