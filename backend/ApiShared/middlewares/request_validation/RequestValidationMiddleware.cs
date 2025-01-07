using ApiShared.interfaces;
using Microsoft.AspNetCore.Http;
using SharedKernel.Common.errors;

namespace ApiShared.middlewares.request_validation;
public class RequestValidationMiddleware
{
    private readonly RequestDelegate _next;

    public RequestValidationMiddleware(RequestDelegate next) {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context) {
        var endpoint = context.GetEndpoint();

        if (endpoint != null) {
            var metadata = endpoint.Metadata
                .OfType<BodyToValidateTypeMetaData>()
                .FirstOrDefault();

            if (metadata != null) {
                //for not application/json type requests
                if (!context.Request.ContentType?.StartsWith("application/json", StringComparison.OrdinalIgnoreCase) ?? true) {
                    var err = new Err("Invalid Content-Type. Expected application/json.", source: ErrorSource.Client);
                    var badRequestResult = CustomResults.ErrorResponse(err);
                    await badRequestResult.ExecuteAsync(context);
                    return;
                }
                //for empty body requests
                long requestLength = context.Request.ContentLength ?? 0;
                if (requestLength == 0) {
                    var err = new Err("Request body cannot be empty when Content-Type is application/json.", source: ErrorSource.Client);
                    var badRequestResult = CustomResults.ErrorResponse(err);
                    await badRequestResult.ExecuteAsync(context);
                    return;
                }

                var requestType = metadata.RequestType;
                var request = await context.Request.ReadFromJsonAsync(requestType);

                if (request is IRequestWithValidationNeeded validatableRequest) {
                    RequestValidationResult validationResult = validatableRequest.Validate();

                    if (validationResult.AnyErrors(out var errsArray)) {
                        var badRequestResult = CustomResults.ErrorResponse(errsArray);
                        await badRequestResult.ExecuteAsync(context);
                        return;
                    }
                }
                context.Items["ValidatedRequest"] = request;

            }
        }

        await _next(context);
    }
}
