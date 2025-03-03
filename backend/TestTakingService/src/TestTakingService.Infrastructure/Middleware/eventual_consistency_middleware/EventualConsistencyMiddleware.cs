using MediatR;
using Microsoft.AspNetCore.Http;
using TestTakingService.Infrastructure.Persistence;

namespace TestTakingService.Infrastructure.Middleware.eventual_consistency_middleware;

internal class EventualConsistencyMiddleware
{

    private readonly RequestDelegate _next;

    public EventualConsistencyMiddleware(RequestDelegate next) {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IPublisher publisher, TestTakingDbContext dbContext) {
        var transaction = await dbContext.Database.BeginTransactionAsync();
        context.Response.OnCompleted(async () => {
            try {
                await transaction.CommitAsync();
            } catch (Exception) {
                await transaction.RollbackAsync();
            } finally {
                await transaction.DisposeAsync();
            }
        });

        await _next(context);
    }
}
