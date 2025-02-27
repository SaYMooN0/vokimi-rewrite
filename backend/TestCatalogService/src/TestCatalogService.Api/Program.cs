using ApiShared;
using System.Text.Json.Serialization;
using SharedKernel.Common.domain.entity;
using SharedUserRelationsContext;
using TestCatalogService.Api.Endpoints.view_test;
using TestCatalogService.Api.Endpoints.view_test.comments;
using TestCatalogService.Application;
using TestCatalogService.Domain.AppUserAggregate;
using TestCatalogService.Infrastructure;
using TestCatalogService.Infrastructure.Persistence;

namespace TestCatalogService.Api;

public class Program
{
    public static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddOpenApi();
        builder.Services
            .AddAuthTokenConfig(builder.Configuration)
            .AddApplication(builder.Configuration)
            .AddInfrastructure(builder.Configuration)
            .AddSharedUserRelationsContext(builder.Configuration)
            .ConfigureHttpJsonOptions(options => {
                options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
        var app = builder.Build();

        if (app.Environment.IsDevelopment()) {
            app.MapOpenApi();
        }

        app.AddExceptionHandlingMiddleware();
        app.UseHttpsRedirection();

        MapHandlers(app);
        
        using (var scope = app.Services.CreateScope()) {
            var services = scope.ServiceProvider;
            try {
                var appDbContext = services.GetRequiredService<TestCatalogDbContext>();
                appDbContext.Database.EnsureDeleted();
                appDbContext.Database.EnsureCreated();
                appDbContext.AppUsers.Add(new AppUser(new AppUserId(new("01947086-ae53-7834-8d6c-56cdb1bbb587"))));
                appDbContext.SaveChanges();
            } catch (Exception ex) {
                app.Logger.LogError(ex, "An error occurred while initializing the database.");
                throw;
            }
        }
        
        app.Run();
    }

    private static void MapHandlers(WebApplication app) {
        app.MapGroup("/viewTest/{testId}/").MapViewTestRootHandlers();
        app.MapGroup("/viewTest/{testId}/ratings").MapViewTestRatingsHandlers();
        app.MapGroup("/viewTest/{testId}/comments").MapViewTestRootCommentsHandlers();
        app.MapGroup("/viewTest/{testId}/{commentId}").MapViewTestCommentActionsHandlers();
        app.MapGroup("/viewTest/{testId}/{commentId}/answers").MapViewTestCommentAnswersHandlers();
    }
}