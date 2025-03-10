using SharedKernel.Common;
using SharedKernel.Common.interfaces;

namespace TestCatalogService.Domain.UnitTests;

public static class TestsSharedConsts
{
    public static IDateTimeProvider DateTimeProviderInstance = new UtcDateTimeProvider();
}