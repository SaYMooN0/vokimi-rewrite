using ApiShared;
using ApiShared.middlewares.request_validation;
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

        endpoints.MapGet("hello", () => "Hello World!");

        endpoints.MapPost("/register", Register)
            .WithRequestValidationMetaData<RegisterUserRequest>();
        endpoints.MapPost("/login", Login)
            .WithRequestValidationMetaData<LoginUserRequest>();
        endpoints.MapPost("/confirm", ConfirmRegistration)
            .WithRequestValidationMetaData<ConfirmRegistrationRequest>();

        return endpoints;
    }

    private static async Task<IResult> Register(
        [FromBody] RegisterUserRequest request,
        ISender mediator
    ) {
        var command = new AddNewUnconfirmedAppUserCommand(request.Email, request.Password);
        ErrOrNothing result = await mediator.Send(command);

        return CustomResults.FromErrOrNothing(
            result,
            () => Results.Ok()
        );
    }

    private static async Task<IResult> Login(
        [FromBody] LoginRequest request,
        ISender mediator
    ) {
        var command = new AuthenticateUserCommand(request.Email, request.Password);
        var result = await mediator.Send(command);

        throw new NotImplementedException();
    }

    private static async Task<IResult> ConfirmRegistration(
        [FromBody] ConfirmRegistrationRequest request,
        ISender mediator
    ) {
        var command = new ConfirmUserRegistrationCommand();
        ErrOrNothing result = await mediator.Send(command);

        throw new NotImplementedException();
    }

}
