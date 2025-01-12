using ApiShared;
using TestCreationService.Api.Endpoints.test_creation;
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
            app.MapGroup("/generic_tests").MapGenericFormatTestCreationHandlers();
        }
    }
}
