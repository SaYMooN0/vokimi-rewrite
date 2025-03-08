using System.Text.Json.Serialization;
using ApiShared;
using SharedUserRelationsContext;
using TestManagingService.Api.Endpoints;
using TestManagingService.Api.Endpoints.feedback;
using TestManagingService.Api.Endpoints.statistics;
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
        app.MapGroup("/{testId}/actions").MapTestActionsHandlers();
        
        app.MapGroup("/{testId}/overall").MapManageTestOverallHandlers();
        app.MapGroup("/{testId}/statistics").MapManageTestSharedStatisticsHandlers();
        app.MapGroup("/{testId}/tags").MapManageTestTagsHandlers();
        
        app.MapGroup("/{testId}/general/feedback").MapManageGeneralTestFeedbackHandlers();
        app.MapGroup("/{testId}/general/statistics").MapManageGeneralTestStatisticsHandlers();
    }
}