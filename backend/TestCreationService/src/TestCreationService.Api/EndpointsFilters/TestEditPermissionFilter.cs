
using TestCreationService.Application.Common.interfaces.repositories;

namespace TestCreationService.Api.EndpointsFilters;

public class TestEditPermissionFilter : IEndpointFilter
{
    private readonly IBaseTestsRepository _baseTestsRepository;

    public TestEditPermissionFilter(IBaseTestsRepository baseTestsRepository) {
        _baseTestsRepository = baseTestsRepository;
    }

    public ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next) {
        throw new NotImplementedException();
    }
}
