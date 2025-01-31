using ApiShared;
using SharedKernel.Common.EntityIds;
using System;
using System.Text.Json.Serialization;
using TestCreationService.Api.Endpoints;
using TestCreationService.Api.Endpoints.test_creation.formats_shared;
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

            app.Run();
        }
        private static void MapHandlers(WebApplication app) {
            app.MapGroup("/newTestInitialization").MapNewTestInitializationHandlers();
            
            app.MapGroup("/testCreation/{testId}").MapFormatsSharedTestCreationHandlers();
            app.MapGroup("/testCreation/{testId}/styles").MapTestCreationStylesHandlers();
            app.MapGroup("/testCreation/{testId}/tags").MapTestCreationTagsHandlers();

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
