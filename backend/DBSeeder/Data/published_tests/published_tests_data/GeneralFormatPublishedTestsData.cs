using System.Collections.Immutable;
using DBSeeder.Data.published_tests.published_tests_data.instances;
using DBSeeder.Data.published_tests.test_instances.general_format_test;
using DBSeeder.Data.users;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.tests.general_format;


namespace DBSeeder.Data.published_tests.published_tests_data;

internal static class GeneralFormatPublishedTestsData
{
    public static readonly AppUserId Test1_CreatorId = AppUsersData.User1.Id;
    public static readonly TestId Test1_Id = AppUsersData.User1.PublishedCreatedTestIds[0];

    public static readonly ImmutableArray<AppUserId> Test1_EditorIds = [
        AppUsersData.User2.Id,
        AppUsersData.User3.Id
    ];

    private static readonly ImmutableHashSet<TestTagId> Test1_TestTagIds = [
        new("BestTestEver"), new("TestNumber1")
    ];

    public static readonly GeneralFormatPublishedTestInstance Test1 = GeneralFormatPublishedTestInstance.Create(
        Test1_Id,
        Test1_CreatorId,
        Test1_EditorIds,
        "Test number 1 name",
        "coverImage.png",
        "description of the test",
        Test1_TestTagIds,
        TestCreationService.Domain.TestAggregate.formats_shared.TestInteractionsAccessSettings.CreateNew(),
        GeneralFormatPublishedTestsDataTest1.AllQuestions,
        shuffleQuestions: true,
        GeneralFormatPublishedTestsDataTest1.AllResults,
        GeneralTestFeedbackOption.Disabled.Instance
    );

    public static readonly ImmutableArray<GeneralFormatPublishedTestInstance> AllTests = [
        Test1
    ];
}