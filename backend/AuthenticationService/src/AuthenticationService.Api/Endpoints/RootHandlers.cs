using ApiShared;
using ApiShared.extensions;
using ApiShared.extensions.endpoints_extensions;
using AuthenticationService.Api.Contracts.AppUsers.requests;
using AuthenticationService.Api.Contracts.UnconfirmedAppUsers.requests;
using AuthenticationService.Application.UnconfirmedAppUsers.commands;
using AuthenticationService.Domain.Common;
using MediatR;
using SharedKernel.Common.errors;
namespace AuthenticationService.Api.Endpoints;

internal static class RootHandlers
{
    internal static IEndpointRouteBuilder MapRootHandlers(this IEndpointRouteBuilder endpoints) {

        endpoints.MapGet(".hello", () => "Hello World!");

        endpoints.MapPost("/register", Register)
            .WithRequestValidationMetaData<RegisterUserRequest>();
        endpoints.MapPost("/login", Login)
            .WithRequestValidationMetaData<LoginUserRequest>();
        endpoints.MapPost("/confirm", ConfirmRegistration)
            .WithRequestValidationMetaData<ConfirmRegistrationRequest>();
        endpoints.MapPost("/logout", Logout);

        return endpoints;
    }

    private static async Task<IResult> Register(
            HttpContext httpContext,
            ISender mediator
    ) {

        RegisterUserRequest request = httpContext.GetValidatedRequest<RegisterUserRequest>();
        var command = new AddNewUnconfirmedAppUserCommand(request.Email, request.Password);
        ErrListOrNothing result = await mediator.Send(command);

        return CustomResults.FromErrListOrNothing(
            result,
            () => Results.Ok()
        );
    }



    private static async Task<IResult> ConfirmRegistration(
        HttpContext httpContext,
        ISender mediator
    ) {
        ConfirmRegistrationRequest request = httpContext.GetValidatedRequest<ConfirmRegistrationRequest>();
        UnconfirmedAppUserId unconfirmedUserId = new(new(request.UserId));
        var command = new ConfirmUserRegistrationCommand(unconfirmedUserId, request.ConfirmationString);
        ErrOr<string> result = await mediator.Send(command);

        return CustomResults.FromErrOr(
            result,
            (token) => CustomResults.ErrorResponse(Err.ErrFactory.NotImplemented("login is not implemented"))
            //Results.Ok().Cookies.Append(AuthCookieName, token, AuthCookieOptions())
        );
    }
    private static async Task<IResult> Login(
        HttpContext httpContext,
        ISender mediator
    ) {
        LoginUserRequest request = httpContext.GetValidatedRequest<LoginUserRequest>();
        var command = new AuthenticateUserCommand(request.Email, request.Password);
        var result = await mediator.Send(command);

        return CustomResults.FromErrOr(
            result,
            (token) => CustomResults.ErrorResponse(Err.ErrFactory.NotImplemented("login is not implemented"))
        );
    }
    private static async Task<IResult> Logout(
       HttpContext httpContext,
       ISender mediator
   ) {
        var cookieOptions = new CookieOptions {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(-1)
        };

        //Cookies.Append("AuthToken", "", cookieOptions);
        return CustomResults.ErrorResponse(Err.ErrFactory.NotImplemented("logout is not implemented"));
    }
    private static CookieOptions AuthCookieOptions() => new CookieOptions {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.Strict,
        Expires = DateTime.UtcNow.AddDays(30)
    };
    private const string AuthCookieName = "VokimiToken";
}
