using TestCreationService.Api.EndpointsFilters;

namespace TestCreationService.Api.Extensions;

internal static class RouteGroupBuilderExtensions
{
    public static RouteGroupBuilder GroupTestEditPermissionRequired(this RouteGroupBuilder builder) {
        return builder.AddEndpointFilter<TestEditPermissionFilter>();
    }
    public static RouteGroupBuilder GroupOnlyByTestCreator(this RouteGroupBuilder builder) {
        return builder.AddEndpointFilter<UserIsTestCreatorFilter>();
    }  
    public static RouteGroupBuilder GroupCheckIfGeneralTestQuestionInProvidedTest(this RouteGroupBuilder builder) {
        return builder.AddEndpointFilter<GeneralTestQuestionIsInTestFilter>();
    } 
    public static RouteGroupBuilder GroupCheckIfGeneralTestAnswerInProvidedQuestion(this RouteGroupBuilder builder) {
        return builder.AddEndpointFilter<GeneralTestAnswerIsInQuestionFilter>();
    }
}
