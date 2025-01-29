using ApiShared;
using SharedKernel.Common.EntityIds;
using System;
using System.Text.Json.Serialization;
using TestCreationService.Api.Endpoints;
using TestCreationService.Api.Endpoints.test_creation;
using TestCreationService.Api.Endpoints.test_creation.general;
using TestCreationService.Application;
using TestCreationService.Domain.AppUserAggregate;
using TestCreationService.Infrastructure;
using TestCreationService.Infrastructure.Persistence;

namespace TestCreationService.Api
{
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
                    var appDbContext = services.GetRequiredService<TestCreationDbContext>();
                    appDbContext.Database.EnsureDeleted();
                    appDbContext.Database.EnsureCreated();
                    appDbContext.AppUsers.Add(new AppUser(new AppUserId(new("01947086-ae53-7834-8d6c-56cdb1bbb587"))));
                    appDbContext.AppUsers.Add(new AppUser(new AppUserId(new("01944d13-85cd-776a-98ab-e60fccc5928f"))));
                    appDbContext.AppUsers.Add(new AppUser(new AppUserId(new("019456b8-497b-7e5b-b6f8-688b1e595ac0"))));
                    appDbContext.AppUsers.Add(new AppUser(new AppUserId(new("019456ca-8673-7907-b43c-83f7db6a1d57"))));
                    appDbContext.AppUsers.Add(new AppUser(new AppUserId(new("019456d5-6292-7ee1-a85a-05b24c1a25ee"))));
                    appDbContext.SaveChanges();
                } catch (Exception ex) {
                    app.Logger.LogError(ex, "An error occurred while initializing the database.");
                    throw;
                }
            }
            app.Run();
        }
        private static void MapHandlers(WebApplication app) {
            app.MapGroup("/newTestInitialization").MapNewTestInitializationHandlers();
            
            app.MapGroup("/testCreation/{testId}").MapFormatsSharedTestCreationHandlers();
            app.MapGroup("/testCreation/{testId}/styles").MapTestCreationStylesHandlers();

            app.MapGroup("/testCreation/{testId}/general").MapGeneralFormatTestCreationHandlers();
            app.MapGroup("/testCreation/{testId}/general/questions").MapGeneralTestCreationQuestionsHandlers();
            app.MapGroup("/testCreation/{testId}/general/questions/{questionId}").MapGeneralTestCreationQuestionOperationsHandlers();
            app.MapGroup("/testCreation/{testId}/general/questions/{questionId}/answers").MapGeneralTestCreationAnswersHandlers();
            app.MapGroup("/testCreation/{testId}/general/questions/{questionId}/answers/{answerId}").MapGeneralTestCreationAnswerOperationsHandlers();
            app.MapGroup("/testCreation/{testId}/general/results").MapGeneralTestCreationResultsHandlers();
            app.MapGroup("/testCreation/{testId}/general/results/{resultId}/").MapGeneralTestCreationResultOperationHandlers();
            
            //app.MapGroup("/testCreation/{testId}/scoring").MapScoringFormatTestCreationHandlers();
        }
    }
}
