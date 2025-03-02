using System.Collections.Immutable;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.tests;

namespace TestManagingService.Domain.TestAggregate.general_format;

public class GeneralFormatTest : BaseTest
{
    private GeneralFormatTest() { }
    public override TestFormat Format => TestFormat.General;

    private GeneralFormatTest(
        TestId testId,
        AppUserId creatorId,
        ImmutableArray<AppUserId> editorIds,
        DateTime publicationDate
    ) : base(testId, creatorId, editorIds, publicationDate) { }

    public static GeneralFormatTest CreateNew(
        TestId testId,
        AppUserId creatorId,
        ImmutableArray<AppUserId> editorIds,
        DateTime publicationDate
    ) => new(testId, creatorId, editorIds, publicationDate);
}