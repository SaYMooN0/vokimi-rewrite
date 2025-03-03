using Microsoft.AspNetCore.Builder;
using TestManagingService.Infrastructure.Middleware.eventual_consistency_middleware;

namespace TestManagingService.Infrastructure;

public static class RequestPipeline
{
    public static IApplicationBuilder AddInfrastructureMiddleware(this IApplicationBuilder app) {
        app.UseMiddleware<EventualConsistencyMiddleware>();
        return app;
    }
}
