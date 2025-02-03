using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain;
using SharedKernel.Common.general_test_questions;
using SharedKernel.Common.general_test_questions.answer_type_specific_data;
using SharedKernel.Common.tests.general_format;
using System.Collections.Immutable;

namespace SharedKernel.IntegrationEvents.test_publishing;

public record GeneralTestPublishedIntegrationEvent(
    TestId TestId,
    AppUserId CreatorIds,
    ImmutableArray<AppUserId> EditorIds,
    string Name,
    string CoverImage,
    string Description,
    Language Language,
    DateTime PublicationDate,
    TestPublishedInteractionsAccessSettingsDto InteractionsAccessSettings,
    TestPublishedStylesDto Styles,
    string[] Tags,
    TestTakingProcessSettings TestTakingProcessSettings,
    bool ShuffleQuestions,
    GeneralTestPublishedQuestionDto[] Questions,
    GeneralTestPublishedResultDto[] Results
) : BaseTestPublishedIntegrationEvent(TestId, CreatorIds, EditorIds, Name, CoverImage, Description, Language, PublicationDate, InteractionsAccessSettings, Styles, Tags);

public record TestTakingProcessSettings(bool ForceSequentialFlow, GeneralTestFeedbackOption feedback);

public record GeneralTestPublishedResultDto(
    GeneralTestResultId Id,
    string Name,
    string Text,
    string Image
);

public record GeneralTestPublishedQuestionDto(
    GeneralTestQuestionId Id,
    ushort Order,
    string Text,
    string[] Images,
    GeneralTestQuestionTimeLimitOption TimeLimitOption,
    GeneralTestAnswersType AnswersType,
    bool ShuffleAnswers,
    GeneralTestQuestionAnswersCountLimit AnswersCountLimit,
    GeneralTestPublishedAnswerDto[] Answers
);
public record GeneralTestPublishedAnswerDto(
    GeneralTestAnswerId Id,
    GeneralTestAnswerTypeSpecificData TypeSpecificData,
    GeneralTestResultId[] RelatedResultsIds
);