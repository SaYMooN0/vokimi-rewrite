using ApiShared.interfaces;

namespace TestCreationService.Api.Contracts.Tests.test_creation.general_format.questions;

public class GeneralTestQuestionsOrderUpdateRequest : IRequestWithValidationNeeded
{
    public Dictionary<string, ushort> QuestionsOrder { get; init; } = new(); // <questionId, position>
    public RequestValidationResult Validate() {
        throw new NotImplementedException();
    }
}
