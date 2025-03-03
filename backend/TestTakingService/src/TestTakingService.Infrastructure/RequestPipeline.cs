using Microsoft.AspNetCore.Builder;
using TestTakingService.Infrastructure.Middleware.eventual_consistency_middleware;

namespace TestTakingService.Infrastructure;

public static class RequestPipeline
{
    public static IApplicationBuilder AddInfrastructureMiddleware(this IApplicationBuilder app) {
        app.UseMiddleware<EventualConsistencyMiddleware>();
        return app;
    }
}
