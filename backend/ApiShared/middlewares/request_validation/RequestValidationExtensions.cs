using ApiShared.interfaces;
using Microsoft.AspNetCore.Builder;

namespace ApiShared.middlewares.request_validation;

public static class RequestValidationExtensions
{
    public static IEndpointConventionBuilder WithRequestValidationMetaData<T>(this IEndpointConventionBuilder builder)
        where T : class, IRequestWithValidationNeeded {
        return builder.WithMetadata(new BodyToValidateTypeMetaData(typeof(T)));
    }
}