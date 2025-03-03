using MediatR;
using Microsoft.AspNetCore.Http;
using TestCatalogService.Infrastructure.Persistence;

namespace TestCatalogService.Infrastructure.Middleware.eventual_consistency_middleware;

internal class EventualConsistencyMiddleware
{
    private readonly RequestDelegate _next;

    public EventualConsistencyMiddleware(RequestDelegate next) {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IPublisher publisher, TestCatalogDbContext dbContext) {
        var transaction = await dbContext.Database.BeginTransactionAsync();
        context.Response.OnCompleted(async () => {
            try {
                await transaction.CommitAsync();
            }
            catch (Exception) {
                await transaction.RollbackAsync();
            }
            finally {
                await transaction.DisposeAsync();
            }
        });

        await _next(context);
    }
}