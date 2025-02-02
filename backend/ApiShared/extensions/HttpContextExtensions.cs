using ApiShared.interfaces;
using Microsoft.AspNetCore.Http;
using SharedKernel.Common.domain;
using SharedKernel.Common.errors;

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
    public static AppUserId GetAuthenticatedUserId(this HttpContext context) {
        if (!context.Items.TryGetValue("AppUserId", out var userIdObj) || userIdObj is not AppUserId userId) {
            throw new UnauthorizedAccessException("User is not authenticated or AppUserId is missing");
        }

        return userId;
    }
    public static TestId GetTestIdFromRoute(this HttpContext context) {
        var testIdString = context.Request.RouteValues["testId"]?.ToString() ?? "";
        if (!Guid.TryParse(testIdString, out var testGuid)) {
            throw new ErrCausedException(Err.ErrFactory.InvalidData("Invalid test id"));
        }

        return new TestId(testGuid);
    }
}
