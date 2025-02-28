using MediatR;
using Microsoft.AspNetCore.Http;
using TestManagingService.Infrastructure.Persistence;

namespace TestManagingService.Infrastructure.Middleware.eventual_consistency_middleware;

internal class EventualConsistencyMiddleware
{

    private readonly RequestDelegate _next;

    public EventualConsistencyMiddleware(RequestDelegate next) {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IPublisher publisher, TestManagingDbContext dbContext) {
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
