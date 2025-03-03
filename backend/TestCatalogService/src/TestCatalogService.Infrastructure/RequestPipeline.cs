using Microsoft.AspNetCore.Builder;
using TestCatalogService.Infrastructure.Middleware.eventual_consistency_middleware;

namespace TestCatalogService.Infrastructure;

public static class RequestPipeline
{
    public static IApplicationBuilder AddInfrastructureMiddleware(this IApplicationBuilder app) {
        app.UseMiddleware<EventualConsistencyMiddleware>();
        return app;
    }
}