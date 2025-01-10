using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SharedKernel.Common.errors;
using SharedKernel.Common.exceptions;

namespace ApiShared.middlewares.exceptions_handling
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next) {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context) {
            try {
                await _next(context);
            } catch (ErrCausedException ex) {
                //_logger.LogWarning("Handled ErrCausedException: {Message}", ex);
                var errorResponse = CustomResults.ErrorResponse(ex.Err);
                await errorResponse.ExecuteAsync(context);
                return;
            } catch (Exception ex) {
                //_logger.LogError(ex, "Unhandled exception occurred.");
                var serverError = new Err(
                    message: "Server error occurred. Please try again later.",
                    source: ErrorSource.Server
                );
                var errorResponse = CustomResults.ErrorResponse(serverError);
                await errorResponse.ExecuteAsync(context);
                return;
            }
        }
    }

}
