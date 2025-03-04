using System.Text.Json.Serialization;
using ApiShared;
using SharedUserRelationsContext;
using TestTakingService.Api.Endpoints;
using TestTakingService.Application;
using TestTakingService.Infrastructure;
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
            .AddSharedUserRelationsContext(builder.Configuration)
            .ConfigureHttpJsonOptions(options => {
                options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });


        var app = builder.Build();
        app.AddInfrastructureMiddleware();

        if (app.Environment.IsDevelopment()) {
            app.MapOpenApi();
        }

        app.AddExceptionHandlingMiddleware();
        app.UseHttpsRedirection();

        MapHandlers(app);
        app.Run();
    }

    private static void MapHandlers(WebApplication app) {
        app.MapGroup("/{testId}/general/").MapGeneralTestTakingHandlers();
    }
}