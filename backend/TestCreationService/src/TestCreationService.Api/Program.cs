using ApiShared;
using SharedKernel.Common.EntityIds;
using System;
using TestCreationService.Api.Endpoints;
using TestCreationService.Api.Endpoints.test_creation;
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
                .AddInfrastructure(builder.Configuration);
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
            app.MapGroup("/testCreation/{testId}/general").MapGeneralFormatTestCreationHandlers();
            //app.MapGroup("/testCreation/{testId}/scoring").MapScoringFormatTestCreationHandlers();
        }
    }
}
