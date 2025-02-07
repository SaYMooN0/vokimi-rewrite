using System.Text.Json.Serialization;
using TestTakingService.Application;
using TestTakingService.Infrastructure;
using ApiShared;
using TestTakingService.Api.Endpoints;
using TestTakingService.Infrastructure.Persistence;

namespace TestTakingService.Api;

public class Program
{
    public static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddOpenApi();
        builder.Services
            .AddAuthTokenConfig(builder.Configuration)
            .AddApplication(builder.Configuration)
            .AddInfrastructure(builder.Configuration)
            .ConfigureHttpJsonOptions(options => { options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()); });
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
                var appDbContext = services.GetRequiredService<TestTakingDbContext>();
                appDbContext.Database.EnsureDeleted();
                appDbContext.Database.EnsureCreated();
                appDbContext.SaveChanges();
            } catch (Exception ex) {
                app.Logger.LogError(ex, "An error occurred while initializing the database.");
                throw;
            }
        }
        app.Run();
    }

    private static void MapHandlers(WebApplication app) {
        app.MapGroup("/{testId}/general/").MapGeneralTestTakingHandlers();
    }
}
