
using ApiShared.middlewares.exceptions_handling;
using ApiShared.middlewares.request_validation;
using TestCreationService.Api.Endpoints.test_creation;
using TestCreationService.Application;
using TestCreationService.Infrastructure;
namespace TestCreationService.Api
{
    public class Program
    {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            builder.Services.AddOpenApi();
            builder.Services
                .AddApplication(builder.Configuration)
                .AddInfrastructure(builder.Configuration);
            var app = builder.Build();

            if (app.Environment.IsDevelopment()) {
                app.MapOpenApi();
            }

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseHttpsRedirection();
            app.UseMiddleware<RequestValidationMiddleware>();

            MapHandlers(app);

            app.Run();
        }
        private static void MapHandlers(WebApplication app) {
            app.MapGroup("/generic_tests").MapGenericFormatTestCreationHandlers();
        }
    }
}
