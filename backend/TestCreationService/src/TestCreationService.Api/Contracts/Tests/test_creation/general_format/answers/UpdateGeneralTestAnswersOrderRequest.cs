using ApiShared.interfaces;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using TestCreationService.Domain.Common;

namespace TestCreationService.Api.Contracts.Tests.test_creation.general_format.answers;

internal class UpdateGeneralTestAnswersOrderRequest : IRequestWithValidationNeeded
{
    public bool ShuffleAnswers { get; init; }
    public Dictionary<string, int> AnswersOrder { get; init; }

    public RequestValidationResult Validate() {
        if (CreateOrderController().IsErr(out var err)) {
            return err;
        }

        return RequestValidationResult.Success;
    }

    public ErrOr<EntitiesOrderController<GeneralTestAnswerId>> CreateOrderController() {
        if (AnswersOrder.Keys.Any(id => !Guid.TryParse(id, out var _))) {
            return Err.ErrFactory.InvalidData("Answer orders have invalid id format");
        }

        if (AnswersOrder.Values.Any(order => order < 0 || order > ushort.MaxValue)) {
            return Err.ErrFactory.InvalidData("Answer orders have invalid order value");
        }

        return EntitiesOrderController<GeneralTestAnswerId>.CreateNew(
            ShuffleAnswers,
            AnswersOrder.ToDictionary(
                kvp => new GeneralTestAnswerId(new Guid(kvp.Key)),
                kvp => (ushort)kvp.Value
            )
        );
    }
}