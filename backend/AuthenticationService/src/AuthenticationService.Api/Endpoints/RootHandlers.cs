using ApiShared;
using ApiShared.extensions;
using ApiShared.extensions.endpoints_extensions;
using AuthenticationService.Api.Contracts.AppUsers.requests;
using AuthenticationService.Api.Contracts.UnconfirmedAppUsers.requests;
using AuthenticationService.Application.UnconfirmedAppUsers.commands;
using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using OneOf.Types;
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

    private static async Task<IResult> Login(
        HttpContext httpContext,
        ISender mediator
    ) {
        LoginUserRequest request = httpContext.GetValidatedRequest<LoginUserRequest>();
        var command = new AuthenticateUserCommand(request.Email, request.Password);
        var result = await mediator.Send(command);

        return CustomResults.FromErrOr(
            result,
            (success) => {
                //write jwt
                throw new NotImplementedException();
            }
        );
    }

    private static async Task<IResult> ConfirmRegistration(
        HttpContext httpContext,
        ISender mediator
    ) {
        ConfirmRegistrationRequest request = httpContext.GetValidatedRequest<ConfirmRegistrationRequest>();
        var command = new ConfirmUserRegistrationCommand();
        ErrOr<string> result = await mediator.Send(command);
        throw new NotImplementedException();


    }

}
