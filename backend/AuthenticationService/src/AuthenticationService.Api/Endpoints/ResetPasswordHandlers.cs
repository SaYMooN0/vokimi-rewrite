using ApiShared;

namespace AuthenticationService.Api.Endpoints;

internal static class ResetPasswordHandlers
{
    internal static RouteGroupBuilder MapResetPasswordHandlers(this RouteGroupBuilder group) {
        group.MapPost("/createRequest", CreateResetPasswordRequest);
        group.MapPost("/confirm", ConfirmResetPasswordRequest);
        return group;
    }
    private static async Task<IResult> CreateResetPasswordRequest() {
        return CustomResults.NotImplemented(nameof(CreateResetPasswordRequest));
    }  
    private static async Task<IResult> ConfirmResetPasswordRequest() {
        return CustomResults.NotImplemented(nameof(ConfirmResetPasswordRequest));
    }
}
