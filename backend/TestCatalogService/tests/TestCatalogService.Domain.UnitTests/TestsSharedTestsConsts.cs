using SharedKernel.Common;
using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.interfaces;
using SharedKernel.Common.tests.value_objects;
using TestCatalogService.Domain.TestAggregate;
using TestCatalogService.Domain.TestAggregate.formats_shared;
using TestCatalogService.Domain.TestAggregate.general_format;

namespace TestCatalogService.Domain.UnitTests;

public static class TestsSharedTestsConsts
{
    public static readonly AppUserId TestCreatorId = new(Guid.NewGuid());

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
            "Just test name",
            "coverImg.png",
            "Basic description",
            TestCreatorId,
            [],
            DateTime.Now.AddYears(-1).AddDays(-1),
            Language.Eng,
            8,
            8,
            true,
            customInteractionsAccessSettings ?? DefaultInteractionsAccessSettings,
            []
        ).GetSuccess();

    public static IDateTimeProvider DateTimeProviderInstance = new UtcDateTimeProvider();
}