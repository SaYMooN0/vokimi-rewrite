namespace TestCreationService.Api.Endpoints.test_creation;

internal static class GeneralFormatTestCreationHandlers
{
    internal static RouteGroupBuilder MapGeneralFormatTestCreationHandlers(this RouteGroupBuilder group) {
        group.MapPost("/blabla", () => "dsda");
        return group;
    }

}
