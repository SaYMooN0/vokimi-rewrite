using AuthenticationService.Infrastructure.Middleware.eventual_consistency_middleware;
using Microsoft.AspNetCore.Builder;


namespace AuthenticationService.Infrastructure;

public static class RequestPipeline
{
    public static IApplicationBuilder AddInfrastructureMiddleware(this IApplicationBuilder app) {
        app.UseMiddleware<DapperEventualConsistencyMiddleware>();
        return app;
    }
}
