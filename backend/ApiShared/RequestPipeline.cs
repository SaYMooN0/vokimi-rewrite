using ApiShared.middlewares.exceptions_handling;
using Microsoft.AspNetCore.Builder;

namespace ApiShared;

public static class RequestPipeline
{
    public static IApplicationBuilder AddExceptionHandlingMiddleware(this IApplicationBuilder app) {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        return app;
    }
}
