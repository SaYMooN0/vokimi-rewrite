using ApiShared.interfaces;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;

namespace TestCreationService.Api.Contracts.Tests.test_creation.formats_shared;

public record class ChangeTestCreatorRequest(
    string NewCrestorId,
    bool KeepCurrentCreatorAsEditor
) : IRequestWithValidationNeeded
{
    public ErrOr<AppUserId> ParsedNewCreatorId() => Guid.TryParse(NewCrestorId, out var guid) ?
        new AppUserId(guid) :
        Err.ErrFactory.InvalidData("Incorrect creator id", details: "Incorrect creator id format");

    public RequestValidationResult Validate() {
        if (ParsedNewCreatorId().IsErr(out var err)) {
            return err;
        }
        return RequestValidationResult.Success;
    }
}
