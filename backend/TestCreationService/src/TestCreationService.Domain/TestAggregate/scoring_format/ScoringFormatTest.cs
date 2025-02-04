using SharedKernel.Common.domain;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests;
using TestCreationService.Domain.TestAggregate.formats_shared;
using TestCreationService.Domain.TestAggregate.formats_shared.events;
using TestCreationService.Domain.TestAggregate.general_format;

namespace TestCreationService.Domain.TestAggregate.scoring_format;

public class ScoringFormatTest : BaseTest
{
    private ScoringFormatTest() { }
    public override TestFormat Format => TestFormat.Scoring;

    private ScoringFormatTest(
        AppUserId creatorId,
        HashSet<AppUserId> editorIds,
        TestMainInfo mainInfo
    ) : base(TestId.CreateNew(), creatorId, editorIds, mainInfo) { }

    public static ErrOr<ScoringFormatTest>
        CreateNew(AppUserId creatorId, string testName, HashSet<AppUserId> editorIds) {
        var mainInfoCreation = TestMainInfo.CreateNew(testName);
        if (mainInfoCreation.IsErr(out var err)) {
            return err;
        }

        if (editorIds.Contains(creatorId)) {
            editorIds.Remove(creatorId);
        }

        var newTest = new ScoringFormatTest(
            creatorId,
            editorIds.ToHashSet(),
            mainInfoCreation.GetSuccess()
        );
        newTest._domainEvents.Add(new TestEditorsListChangedEvent(newTest.Id, editorIds, []));
        newTest._domainEvents.Add(new NewTestInitializedEvent(newTest.Id, newTest.CreatorId));
        return newTest;
    }

    public override void DeleteTest() {
        throw new ErrCausedException(Err.ErrFactory.NotImplemented());
    }
}