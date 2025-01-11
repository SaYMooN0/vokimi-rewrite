using ApiShared;
using AuthenticationService.Api.Endpoints;
using AuthenticationService.Application;
using AuthenticationService.Infrastructure;

namespace AuthenticationService.Api
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
            app.AddInfrastructureMiddleware();


            MapHandlers(app);

            app.Run();
        }
        private static void MapHandlers(WebApplication app) {
            app.MapRootHandlers();
            app.MapGroup("/resetPassword").MapResetPasswordHandlers();
        }
    }
}
