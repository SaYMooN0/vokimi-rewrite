using ApiShared.interfaces;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests;
using System.Text.Json.Serialization;
using TestCreationService.Domain.Rules;

namespace TestCreationService.Api.Contracts.Tests.test_creation.general_format.questions.add_question;

public class AddGeneralFormatTestQuestionRequest : IRequestWithValidationNeeded
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public GeneralTestAnswersType AnswersType { get; init; }
    public bool IsTimeLimited { get; init; }

    public RequestValidationResult Validate() {
        if (IsTimeLimited && GeneralFormatTestRules.TimeLimitedQuestionsNotAllowedAnswers.Contains(AnswersType)) {
            return Err.ErrFactory.InvalidData(
                message: "Time limited questions are not allowed to have answers of type " + AnswersType,
                details: "You can either remove the time limit or change the answers type"
            );
        }
        return RequestValidationResult.Success;
    }
}
