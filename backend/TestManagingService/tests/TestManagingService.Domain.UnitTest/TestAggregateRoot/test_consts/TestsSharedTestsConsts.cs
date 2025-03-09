using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain.entity;
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

    public static BaseTest CreateBaseTest(TestInteractionsAccessSettings? customInteractionsAccessSettings = null) =>
        GeneralFormatTest.CreateNew(
            TestId.CreateNew(),
            TestCreator, [],
            DateTime.Now.AddYears(-1).AddDays(-1),
            DefaultInteractionsAccessSettings
        );
}