using TestCreationService.Api.EndpointsFilters;

namespace TestCreationService.Api.Extensions;

internal static class RouteGroupBuilderExtensions
{
    public static RouteGroupBuilder GroupTestEditPermissionRequired(this RouteGroupBuilder builder) {
        return builder.AddEndpointFilter<TestEditPermissionFilter>();
    }
    public static RouteGroupBuilder GroupCheckIfGeneralTestQuestionInProvidedTest(this RouteGroupBuilder builder) {
        return builder.AddEndpointFilter<GeneralTestQuestionIsInTestFilter>();
    } 
}
