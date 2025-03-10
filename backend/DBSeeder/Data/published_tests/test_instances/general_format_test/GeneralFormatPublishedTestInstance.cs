using System.Collections.Immutable;
using DBSeeder.Data.published_tests.test_instances.formats_shared;
using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.general_test_questions;
using SharedKernel.Common.tests.formats_shared.interaction_access_settings;
using SharedKernel.Common.tests.formats_shared.test_styles;
using SharedKernel.Common.tests.general_format;
using TestQuestion = TestTakingService.Domain.TestAggregate.general_format.GeneralTestQuestion;
using TestResult = TestTakingService.Domain.TestAggregate.general_format.GeneralTestResult;

namespace DBSeeder.Data.published_tests.test_instances.general_format_test;

public class GeneralFormatPublishedTestInstance : BasePublishedTestInstance
{
    private GeneralFormatPublishedTestInstance(
        TestCatalogService.Domain.TestAggregate.general_format.GeneralFormatTest testCatalogTest,
        TestManagingService.Domain.TestAggregate.general_format.GeneralFormatTest testManagingTest,
        TestTakingService.Domain.TestAggregate.general_format.GeneralFormatTest testTakingTest
    ) : base(testCatalogTest, testManagingTest, testTakingTest) { }

    public static GeneralFormatPublishedTestInstance Create(
        TestId testId,
        AppUserId creatorId,
        ImmutableArray<AppUserId> editorIds,
        string name,
        string coverImg,
        string description,
        ImmutableHashSet<TestTagId> tags,
        ITestInteractionsAccessSettings interactionsAccessSettings,
        IReadOnlyCollection<TestQuestion> questions,
        bool shuffleQuestions,
        IReadOnlyCollection<TestResult> results,
        GeneralTestFeedbackOption feedbackOption,
        Language language = Language.Other,
        AccessLevel accessLevel = AccessLevel.Public,
        DateTime? publicationDate = null
    ) {
        var publicationDateValue = publicationDate ?? DateTime.UtcNow;

        bool anyAudioAnswers = questions.Any(q => q.AnswersType.HasAudio());
        TestCatalogService.Domain.TestAggregate.formats_shared.TestInteractionsAccessSettings
            tISAccessSettings = new(
                interactionsAccessSettings.TestAccess,
                interactionsAccessSettings.AllowRatings,
                interactionsAccessSettings.AllowComments,
                interactionsAccessSettings.AllowTestTakenPosts,
                interactionsAccessSettings.AllowTagsSuggestions
            );
        TestManagingService.Domain.TestAggregate.formats_shared.TestInteractionsAccessSettings
            tMSAccessSettings = new(
                interactionsAccessSettings.TestAccess,
                interactionsAccessSettings.AllowRatings,
                interactionsAccessSettings.AllowComments,
                interactionsAccessSettings.AllowTestTakenPosts,
                interactionsAccessSettings.AllowTagsSuggestions
            );
        
        var testCatalogTest =
            TestCatalogService.Domain.TestAggregate.general_format.GeneralFormatTest.CreateNew(
                testId, name, coverImg, description, creatorId, editorIds, publicationDateValue, language,
                (ushort)questions.Count, (ushort)results.Count, anyAudioAnswers, tISAccessSettings, tags
            ).GetSuccess();

        var testManagingTest = TestManagingService.Domain.TestAggregate.general_format.GeneralFormatTest.CreateNew(
            testId, creatorId, editorIds, publicationDateValue, tMSAccessSettings
        );

        TestStylesSheet styles = TestStylesSheet.CreateNew(testId);
        var testTakingTest = new TestTakingService.Domain.TestAggregate.general_format.GeneralFormatTest(
            testId, creatorId, editorIds.ToImmutableHashSet(), accessLevel, styles, questions,
            shuffleQuestions, results, feedbackOption
        );

        return new GeneralFormatPublishedTestInstance(testCatalogTest, testManagingTest, testTakingTest);
    }
}