using ApiShared.endpoints_filters;
using ApiShared.interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ApiShared.extensions;

public static class EndpointRouteBuilderExtensions
{
    public static RouteHandlerBuilder WithRequestValidation<T>(this RouteHandlerBuilder builder) where T : class, IRequestWithValidationNeeded {
        return builder.AddEndpointFilter<RequestValidationRequiredEndpointFilter<T>>();
    }
    public static RouteHandlerBuilder AuthenticationRequired(this RouteHandlerBuilder builder) {
        return builder.AddEndpointFilter<AuthenticationRequiredEndpointFilter>();
    }
}
