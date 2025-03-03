namespace DBSeeder.Data.published_tests.test_instances.formats_shared;

public abstract class BasePublishedTestInstance
{
    public TestCatalogService.Domain.TestAggregate.BaseTest TestCatalogTest { get; }
    public TestManagingService.Domain.TestAggregate.BaseTest TestManagingTest { get; }
    public TestTakingService.Domain.TestAggregate.BaseTest TestTakingTest { get; }

    protected BasePublishedTestInstance(
        TestCatalogService.Domain.TestAggregate.BaseTest testCatalogTest,
        TestManagingService.Domain.TestAggregate.BaseTest testManagingTest,
        TestTakingService.Domain.TestAggregate.BaseTest testTakingTest
    ) {
        TestCatalogTest = testCatalogTest;
        TestManagingTest = testManagingTest;
        TestTakingTest = testTakingTest;
    }
}