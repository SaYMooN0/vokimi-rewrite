using ApiShared.interfaces;
using Microsoft.AspNetCore.Http;
using SharedKernel.Common.errors;

namespace ApiShared.endpoints_filters;

internal class RequestValidationRequiredEndpointFilter<T> : IEndpointFilter where T : class, IRequestWithValidationNeeded
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next) {
        var httpContext = context.HttpContext;

        ///for not application/json type requests
        if (!httpContext.Request.ContentType?.StartsWith("application/json", StringComparison.OrdinalIgnoreCase) ?? true) {
            var err = new Err("Invalid Content-Type. Expected application/json.", source: ErrorSource.Client);
            return CustomResults.ErrorResponse(err);
        }

        //for empty body requests
        if (httpContext.Request.ContentLength == 0) {
            var err = new Err("Request body cannot be empty when Content-Type is application/json.", source: ErrorSource.Client);
            return CustomResults.ErrorResponse(err);
        }
        T request = null;

        try {
            request = await httpContext.Request.ReadFromJsonAsync<T>();
        } catch (System.Text.Json.JsonException) {
            return CustomResults.ErrorResponse(new Err("Unable to read from json"));
        }

        if (request is not T validatableRequest) {
            var err = new Err("Invalid request body format.", source: ErrorSource.Client);
            return CustomResults.ErrorResponse(err);
        }

        var validationResult = validatableRequest.Validate();
        if (validationResult.AnyErrors(out var errsArray)) {
            return CustomResults.ErrorResponse(errsArray);
        }

        httpContext.Items["ValidatedRequest"] = validatableRequest;

        return await next(context);
    }
}

