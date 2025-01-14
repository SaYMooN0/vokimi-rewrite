using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests;
using TestCreationService.Domain.TestAggregate.formats_shared;
using TestCreationService.Domain.TestAggregate.formats_shared.events;

namespace TestCreationService.Domain.TestAggregate.general_format;

public class GeneralFormatTest : BaseTest
{
    private GeneralFormatTest() { }
    public override TestFormat Format => TestFormat.General;
    public static ErrOr<GeneralFormatTest> CreateNew(AppUserId creatorId, string testName, IEnumerable<AppUserId> editorIds) {
        var mainInfoCreation = TestMainInfo.CreateNew(testName);
        if (mainInfoCreation.IsErr(out var err)) {
            return err;
        }

        var editorIdsList = editorIds.ToList();
        if (editorIdsList.Any()) {
            if (editorIdsList.Contains(creatorId)) {
                editorIdsList.Remove(creatorId);
            }
        }
        var newTest = new GeneralFormatTest(
            creatorId,
            editorIdsList.ToHashSet(),
            mainInfoCreation.GetSuccess()
        );
        newTest._domainEvents.Add(new TestEditorsListChangedEvent(newTest.Id, editorIdsList));
        return newTest;
    }
    private GeneralFormatTest(
        AppUserId creatorId,
        HashSet<AppUserId> editorIds,
        TestMainInfo mainInfo
    ) : base(TestId.CreateNew(), creatorId, editorIds, mainInfo, TestSettings.Deafult, TestStyles.Default) { }
}
