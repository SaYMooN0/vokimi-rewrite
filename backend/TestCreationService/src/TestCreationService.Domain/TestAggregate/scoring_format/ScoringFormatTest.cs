using SharedKernel.Common.EntityIds;
using SharedKernel.Common.tests;
using TestCreationService.Domain.TestAggregate.formats_shared;

namespace TestCreationService.Domain.TestAggregate.scoring_format;

public class ScoringFormatTest : BaseTest
{
    private ScoringFormatTest() { }
    public override TestFormat Format => TestFormat.Scoring;
    private ScoringFormatTest(
       AppUserId creatorId,
       HashSet<AppUserId> editorIds,
       TestMainInfo mainInfo
   ) : base(TestId.CreateNew(), creatorId, editorIds, mainInfo, TestSettings.Deafult, TestStyles.Default) { }

}
