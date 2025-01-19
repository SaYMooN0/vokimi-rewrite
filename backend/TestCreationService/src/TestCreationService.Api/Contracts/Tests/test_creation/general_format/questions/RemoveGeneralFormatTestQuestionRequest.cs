using ApiShared.interfaces;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;

namespace TestCreationService.Api.Contracts.Tests.test_creation.general_format.questions;

internal class RemoveGeneralFormatTestQuestionRequest : IRequestWithValidationNeeded
{
    public string QuestionId { get; init; }
    public RequestValidationResult Validate() {
        if (!Guid.TryParse(QuestionId, out _)) {
            return Err.ErrFactory.InvalidData(
                message: "Unable to parse question id",
                details: "Question id is not a valid format"
            );
        };
        return RequestValidationResult.Success;
    }
    public GeneralTestQuestionId ParsedQuestionId => new(Guid.Parse(QuestionId));
}
