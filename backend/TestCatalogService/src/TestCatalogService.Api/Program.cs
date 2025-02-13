using ApiShared;
using System.Text.Json.Serialization;
using SharedUserRelationsContext;
using TestCatalogService.Api.Endpoints.view_test;
using TestCatalogService.Application;
using TestCatalogService.Infrastructure;

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

        app.Run();
    }

    private static void MapHandlers(WebApplication app) {
        app.MapGroup("/viewTest/{testId}/").MapViewTestRootHandlers();
        app.MapGroup("/viewTest/{testId}/ratings").MapViewTestRatingsHandlers();
        app.MapGroup("/viewTest/{testId}/comments").MapViewTestCommentsHandlers();
    }
}