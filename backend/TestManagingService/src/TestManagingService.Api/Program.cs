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

        if (app.Environment.IsDevelopment()) {
            app.MapOpenApi();
        }

        app.AddExceptionHandlingMiddleware();
        app.UseHttpsRedirection();

        MapHandlers(app);
        
        // using (var scope = app.Services.CreateScope()) {
        //     var services = scope.ServiceProvider;
        //     try {
        //         var appDbContext = services.GetRequiredService<TestManagingDbContext>();
        //         appDbContext.Database.EnsureDeleted();
        //         appDbContext.Database.EnsureCreated();
        //         appDbContext.AppUsers.Add(new AppUser(new AppUserId(new("01947086-ae53-7834-8d6c-56cdb1bbb587"))));
        //         appDbContext.SaveChanges();
        //     } catch (Exception ex) {
        //         app.Logger.LogError(ex, "An error occurred while initializing the database.");
        //         throw;
        //     }
        // }
        
        app.Run();
    }

    private static void MapHandlers(WebApplication app) {
        app.MapGroup("/manageTest/{testId}/overall").MapManageTestOverallHandlers();
        app.MapGroup("/manageTest/{testId}/feedback").MapManageTestFeedbackHandlers();
        app.MapGroup("/manageTest/{testId}/statistics").MapManageTestStatisticsHandlers();
        app.MapGroup("/manageTest/{testId}/tags").MapManageTestTagsHandlers();
    }
}