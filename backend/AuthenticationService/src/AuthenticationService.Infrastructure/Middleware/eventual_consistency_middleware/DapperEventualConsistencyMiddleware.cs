using AuthenticationService.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Transactions;

namespace AuthenticationService.Infrastructure.Middleware.eventual_consistency_middleware;

public class DapperEventualConsistencyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public DapperEventualConsistencyMiddleware(RequestDelegate next, IDbConnectionFactory dbConnectionFactory) {
        _next = next;
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task InvokeAsync(HttpContext context) {
        if (!(context.RequestServices.GetService(typeof(UnitOfWork)) is UnitOfWork unitOfWork)) {
            throw new InvalidOperationException("UnitOfWork service is not registered.");
        }

        var connection = await _dbConnectionFactory.CreateConnectionAsync();
        unitOfWork.BeginTransaction(connection);
        context.Response.OnCompleted(async () => {
            try {

                var publisher = context.RequestServices.GetRequiredService<IPublisher>();
                await unitOfWork.PublishDomainEventsAsync(publisher);
                unitOfWork.Commit();
            } catch (Exception) {
                unitOfWork.Rollback();

            } finally {
                connection.Close();
            }
        });
        await _next(context);

        //catch (EventualConsistencyException) {
        //    // handle eventual consistency exception
        //}
    }

}
