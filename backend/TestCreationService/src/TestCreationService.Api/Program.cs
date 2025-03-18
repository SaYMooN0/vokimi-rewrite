using System.Text.Json.Serialization;
using ApiShared;
using TestCreationService.Api.Endpoints;
using TestCreationService.Api.Endpoints.formats_shared;
using TestCreationService.Api.Endpoints.general_format;
using TestCreationService.Api.Endpoints.general_format.answers;
using TestCreationService.Api.Endpoints.general_format.questions;
using TestCreationService.Api.Endpoints.general_format.results;
using TestCreationService.Api.Endpoints.tier_list_format;
using TestCreationService.Api.Endpoints.tier_list_format.items;
using TestCreationService.Api.Endpoints.tier_list_format.tiers;
using TestCreationService.Application;
using TestCreationService.Infrastructure;

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
            app.MapGroup("/newTestInitialization").MapNewTestInitializationHandlers();

            app.MapGroup("/testCreation/{testId}").MapFormatsSharedTestCreationHandlers();
            app.MapGroup("/testCreation/{testId}/styles").MapTestCreationStylesHandlers();
            app.MapGroup("/testCreation/{testId}/tags").MapTestCreationTagsHandlers();

            //general format test
            app.MapGroup("/testCreation/{testId}/general")
                .MapGeneralFormatTestCreationHandlers();
            app.MapGroup("/testCreation/{testId}/general/p")
                .MapGeneralTestPublishingHandlers();
            app.MapGroup("/testCreation/{testId}/general/questions")
                .MapGeneralTestCreationQuestionsHandlers();
            app.MapGroup("/testCreation/{testId}/general/questions/{questionId}")
                .MapGeneralTestCreationQuestionOperationsHandlers();
            app.MapGroup("/testCreation/{testId}/general/questions/{questionId}/answers")
                .MapGeneralTestCreationAnswersHandlers();
            app.MapGroup("/testCreation/{testId}/general/questions/{questionId}/answers/{answerId}")
                .MapGeneralTestCreationAnswerOperationsHandlers();
            app.MapGroup("/testCreation/{testId}/general/results")
                .MapGeneralTestCreationResultsHandlers();
            app.MapGroup("/testCreation/{testId}/general/results/{resultId}/")
                .MapGeneralTestCreationResultOperationHandlers();

            //tier list format test
            app.MapGroup("/testCreation/{testId}/tierList")
                .MapTierListFormatTestCreationHandlers();
            app.MapGroup("/testCreation/{testId}/tierList/p")
                .MapTierListTestPublishingHandlers();
            app.MapGroup("/testCreation/{testId}/tierList/tiers")
                .MapTierListTestCreationTiersHandlers();
            app.MapGroup("/testCreation/{testId}/tierList/tiers/{tierId}")
                .MapTierListTestCreationTierOperationsHandlers();
            app.MapGroup("/testCreation/{testId}/tierList/items")
                .MapTierListTestCreationItemsHandlers();
            app.MapGroup("/testCreation/{testId}/tierList/items/{itemId}")
                .MapTierListTestCreationItemOperationsHandlers();
        }
    }
}