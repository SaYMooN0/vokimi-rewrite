using Microsoft.AspNetCore.Builder;
using TestCreationService.Infrastructure.Middleware.eventual_consistency_middleware;

namespace TestCreationService.Infrastructure;

public static class RequestPipeline
{
    public static IApplicationBuilder AddInfrastructureMiddleware(this IApplicationBuilder app) {
        app.UseMiddleware<EventualConsistencyMiddleware>();
        return app;
    }
}
