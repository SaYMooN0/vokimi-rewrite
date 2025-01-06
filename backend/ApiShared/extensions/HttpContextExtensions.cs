using ApiShared.interfaces;
using Microsoft.AspNetCore.Http;

namespace ApiShared.extensions;

public static class HttpContextExtensions
{
    public static T GetValidatedRequest<T>(this HttpContext context) where T : class, IRequestWithValidationNeeded {
        if (!context.Items.TryGetValue("ValidatedRequest", out var validatedRequest)) {
            throw new InvalidDataException("Trying to access validated request on the request that has not passed the validation");
        }
        if (validatedRequest is T request) {
            return request;
        }
        throw new InvalidCastException("Request type mismatch");
    }
}
