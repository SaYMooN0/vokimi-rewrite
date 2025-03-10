using SharedKernel.Common;
using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.interfaces;
using SharedKernel.Common.tests.value_objects;
using TestManagingService.Domain.TestAggregate;
using TestManagingService.Domain.TestAggregate.formats_shared;
using TestManagingService.Domain.TestAggregate.general_format;

namespace TestManagingService.Domain.UnitTest.TestAggregateRoot.test_consts;

public static class TestsSharedTestsConsts
{
    public static readonly AppUserId TestCreator = new(Guid.NewGuid());

    public static readonly TestInteractionsAccessSettings DefaultInteractionsAccessSettings =
        new TestInteractionsAccessSettings(
            AccessLevel.Public,
            ResourceAvailabilitySetting.EnabledPublic,
            ResourceAvailabilitySetting.EnabledPublic,
            true,
            true
        );


    public static BaseTest CreateTest(
        TestInteractionsAccessSettings? customInteractionsAccessSettings = null,
        TestTagId[]? testTags = null,
        TestTagId[]? tagSuggestions = null,
        TestTagId[]? bannedTags = null
    ) {
        BaseTest test = GeneralFormatTest.CreateNew(
            TestId.CreateNew(),
            TestCreator, [],
            DateTime.Now.AddYears(-1).AddDays(-1),
            customInteractionsAccessSettings ?? DefaultInteractionsAccessSettings
        );
        if (testTags is not null) {
            var result = test.UpdateTags(testTags.ToHashSet());
            if (result.IsErr()) {
                throw new Exception($"Incorrect tags to set");
            }
        }

        if (tagSuggestions is not null) {
            var result = test.AddTagSuggestions(
                tagSuggestions.ToHashSet(), DateTimeProviderInstance
            );
            if (result.IsErr()) {
                throw new Exception($"Incorrect tags to add suggestion");
            }
        }

        if (bannedTags is not null) {
            var result = test.AddTagSuggestions(
                tagSuggestions.ToHashSet(), DateTimeProviderInstance
            );
            if (result.IsErr()) {
                throw new Exception($"Incorrect tags to add suggestion before banning");
            }

            result = test.DeclineAndBanTagSuggestions(
                tagSuggestions.ToHashSet()
            );
            if (result.IsErr()) {
                throw new Exception($"Incorrect tags to ban");
            }
        }

        return test;
    }

    public static IDateTimeProvider DateTimeProviderInstance = new UtcDateTimeProvider();
}