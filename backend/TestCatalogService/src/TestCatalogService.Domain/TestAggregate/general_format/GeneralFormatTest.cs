using System.Collections.Immutable;
using SharedKernel.Common;
using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests;
using TestCatalogService.Domain.Common;
using TestCatalogService.Domain.TestAggregate.formats_shared;
using TestCatalogService.Domain.TestAggregate.formats_shared.events;
using TestCatalogService.Domain.TestTagAggregate.events;

namespace TestCatalogService.Domain.TestAggregate.general_format;

public class GeneralFormatTest : BaseTest
{
    private GeneralFormatTest() { }
    public override TestFormat Format => TestFormat.General;
    public ushort QuestionsCount { get; init; }
    public ushort ResultsCount { get; init; }
    public bool AnyAudioAnswers { get; init; }

    private GeneralFormatTest(
        TestId testId, string name, string coverImg, string description, AppUserId creatorId,
        ImmutableArray<AppUserId> editorIds, DateTime publicationDate, Language language,
        ImmutableHashSet<TestTagId> tags, TestInteractionsAccessSettings interactionsAccessSettings,
        ushort questionsCount, ushort resultsCount, bool anyAudioAnswers
    ) : base(
        testId, name, coverImg, description, creatorId, editorIds,
        publicationDate, language, tags, interactionsAccessSettings
    ) {
        QuestionsCount = questionsCount;
        ResultsCount = resultsCount;
        AnyAudioAnswers = anyAudioAnswers;
    }

    public static ErrOr<GeneralFormatTest> CreateNew(
        TestId testId,
        string name,
        string coverImg,
        string description,
        AppUserId creatorId,
        ImmutableArray<AppUserId> editorIds,
        DateTime publicationDate,
        Language language,
        ushort questionsCount,
        ushort resultsCount,
        bool anyAudioAnswers,
        TestInteractionsAccessSettings interactionsAccessSettings,
        ImmutableHashSet<TestTagId> tags
    ) {
        if (string.IsNullOrWhiteSpace(name)) {
            return Err.ErrFactory.InvalidData("Name is required");
        }

        if (tags.Count > TestTagsRules.MaxTagsForTestCount) {
            tags = tags.Take(TestTagsRules.MaxTagsForTestCount).ToImmutableHashSet();
        }

        GeneralFormatTest newTest = new(
            testId, name, coverImg, description, creatorId, editorIds, publicationDate,
            language, tags, interactionsAccessSettings, questionsCount, resultsCount, anyAudioAnswers
        );
        newTest._domainEvents.Add(new TestTagsChangedEvent(newTest.Id, new HashSet<TestTagId>(), tags));
        newTest._domainEvents.Add(
            new NewPublishedTestCreatedEvent(newTest.Id, creatorId, editorIds.ToImmutableHashSet())
        );
        return newTest;
    }
}