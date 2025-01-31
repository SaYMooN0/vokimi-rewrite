using SharedKernel.Common.EntityIds;

namespace SharedKernel.IntegrationEvents.test_publishing;

public record class GeneralTestPublishedIntegrationEvent(
    TestId Id,
    GeneralTestPublishedResultDto[] Results,
    GeneralTestPublishedQuestionDto[] Questions
//all other settings
) : IIntegrationEvent;

public record GeneralTestPublishedResultDto(GeneralTestResultId Id);
public record GeneralTestPublishedQuestionDto(
    GeneralTestAnswerId Id,
    GeneralTestPublishedAnswerDto[] Answers
);
public record GeneralTestPublishedAnswerDto(GeneralTestAnswerId Id);