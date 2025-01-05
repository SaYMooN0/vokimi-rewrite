using ApiShared.interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Globalization;

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
            }
        }

        await _next(context);
    }
}
