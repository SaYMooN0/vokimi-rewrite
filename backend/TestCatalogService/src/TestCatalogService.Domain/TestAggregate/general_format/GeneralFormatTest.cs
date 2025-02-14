using System.Collections.Immutable;
using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain;
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
        if (string.IsNullOrWhiteSpace(name))
        {
            return Err.ErrFactory.InvalidData("Name is required");
        }

        var newTest = new GeneralFormatTest() {
            Id = testId,
            Name = name,
            CoverImg = coverImg,
            Description = description,
            CreatorId = creatorId,
            EditorIds = editorIds,
            PublicationDate = publicationDate,
            Language = language,
            QuestionsCount = questionsCount,
            ResultsCount = resultsCount,
            AnyAudioAnswers = anyAudioAnswers,
            InteractionsAccessSettings = interactionsAccessSettings,
            Tags = tags
        };
        newTest._domainEvents.Add(new TestTagsChangedEvent(newTest.Id, new HashSet<TestTagId>(), tags));
        newTest._domainEvents.Add(new NewPublishedTestCreatedEvent(newTest.Id, creatorId,
            editorIds.ToImmutableHashSet()));
        return newTest;
    }
}