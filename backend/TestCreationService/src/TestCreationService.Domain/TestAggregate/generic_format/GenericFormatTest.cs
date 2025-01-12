using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using TestCreationService.Domain.TestAggregate.formats_shared;

namespace TestCreationService.Domain.TestAggregate.generic_format;

public class GenericFormatTest : BaseTest
{
    public static ErrOr<GenericFormatTest> CreateNew(AppUserId creatorId, string testName, AppUserId[] editorIds) {
        var mainInfoCreation = TestMainInfo.CreateNew(testName);
        if (mainInfoCreation.IsErr(out var err)) {
            return err;
        }
        List<AppUserId> editorIdsList = editorIds is null ? [] : editorIds.ToList();
        return new GenericFormatTest() {
            Id = TestId.CreateNew(),
            CreatorId = creatorId,
            EditorIds = editorIdsList,
            MainInfo = mainInfoCreation.GetSuccess(),
            Settings = TestSettings.Deafult,
            Styles = TestStyles.Default
        };
    }
}
