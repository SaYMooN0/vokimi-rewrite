using System.Text.Json.Serialization;
using ApiShared;
using SharedUserRelationsContext;
using TestManagingService.Api.Endpoints;
using TestManagingService.Application;
using TestManagingService.Infrastructure;

namespace TestManagingService.Api;

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
        app.MapGroup("/manageTest/{testId}/overall").MapManageTestOverallHandlers();
        app.MapGroup("/manageTest/{testId}/feedback").MapManageTestFeedbackHandlers();
        app.MapGroup("/manageTest/{testId}/statistics").MapManageTestStatisticsHandlers();
        app.MapGroup("/manageTest/{testId}/tags").MapManageTestTagsHandlers();
    }
}