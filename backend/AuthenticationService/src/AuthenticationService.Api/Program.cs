using ApiShared.middlewares.request_validation;
using AuthenticationService.Api.Endpoints;
using AuthenticationService.Application;
using AuthenticationService.Infrastructure;

namespace AuthenticationService.Api
{
    public class Program
    {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthorization();

            builder.Services.AddOpenApi();


            builder.Services
                .AddApplication()
                .AddInfrastructure(builder.Configuration);

            var app = builder.Build();

            if (app.Environment.IsDevelopment()) {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseMiddleware<RequestValidationMiddleware>();

            MapHandlers(app);

            app.Run();
        }
        private static void MapHandlers(WebApplication app) {
            app.MapRootHandlers();
            app.MapGroup("/resetPassword").MapResetPasswordHandlers();
        }
    }
}
