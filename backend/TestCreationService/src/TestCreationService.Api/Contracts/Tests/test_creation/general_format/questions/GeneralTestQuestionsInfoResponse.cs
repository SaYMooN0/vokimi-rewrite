namespace TestCreationService.Api.Contracts.Tests.test_creation.general_format.questions;

public record class GeneralTestQuestionsInfoResponse
{
    internal GeneralTestQuestionBasicData[] Questions { get; init; }

    public GeneralTestQuestionsInfoResponse() {
        Questions = [];
    }
}

internal record class GeneralTestQuestionBasicData
{
    //text, image, answers type, answers count, is multiple, min-max answers count
}