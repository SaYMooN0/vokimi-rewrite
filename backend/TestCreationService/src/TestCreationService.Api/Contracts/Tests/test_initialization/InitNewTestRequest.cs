using ApiShared.interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using TestCreationService.Domain.Rules;

namespace TestCreationService.Api.Contracts.Tests.test_initialization;

internal record class InitNewTestRequest(
    string TestName,
    string[] EditorIds
) : IRequestWithValidationNeeded
{
    public RequestValidationResult Validate() {
        if (EditorIds is null) {
            return Err.ErrFactory.InvalidData("Editors list is not set");
        }
        if (TestRules.CheckTestNameForErrs(TestName).IsErr(out var err)) {
            return err;
        }
        if (EditorIds is not null && EditorIds.Any(eId => !Guid.TryParse(eId, out var _))) {
            return Err.ErrFactory.InvalidData(
                "Editors was not saved correctly. Please try again.",
                details: "If it does not help you can create test without additional editors and add them later"
            );
        }
        return RequestValidationResult.Success;
    }
    public HashSet<AppUserId> GetParsedEditorIdsWithoutCreator(AppUserId creator) =>
        EditorIds
            .Select(id => new AppUserId(new(id)))
            .Where(id => id != creator)
            .ToHashSet();
}
