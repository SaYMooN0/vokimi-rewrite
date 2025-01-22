using ApiShared.interfaces;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using TestCreationService.Domain.Common;

namespace TestCreationService.Api.Contracts.Tests.test_creation.general_format.questions;

internal class UpdateGeneralTestQuestionsOrderRequest : IRequestWithValidationNeeded
{
    public bool ShuffleQuestions { get; init; }
    public Dictionary<string, int> QuestionOrders { get; init; }
    public RequestValidationResult Validate() {
        if (CreateOrderController().IsErr(out var err)) {
            return err;
        }
        return RequestValidationResult.Success;
    }
    public ErrOr<EntitiesOrderController<GeneralTestQuestionId>> CreateOrderController() {
        if (QuestionOrders.Keys.Any(id => !Guid.TryParse(id, out var _))) {
            return Err.ErrFactory.InvalidData("Question orders have invalid id format");
        }
        if (QuestionOrders.Values.Any(order => order < 0 || order > ushort.MaxValue)) {
            return Err.ErrFactory.InvalidData("Question orders have invalid order value");
        }
        return EntitiesOrderController<GeneralTestQuestionId>.CreateNew(
            ShuffleQuestions,
            QuestionOrders.ToDictionary(kvp => new GeneralTestQuestionId(new Guid(kvp.Key)), kvp => (ushort)kvp.Value)
        );
    }
}
