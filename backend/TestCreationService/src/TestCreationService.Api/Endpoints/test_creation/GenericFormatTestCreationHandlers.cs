using ApiShared;
using SharedKernel.Common.errors;

namespace TestCreationService.Api.Endpoints.test_creation
{
    internal static class GenericFormatTestCreationHandlers
    {
        internal static RouteGroupBuilder MapGenericFormatTestCreationHandlers(this RouteGroupBuilder group) {
            group.MapPost("/new", CreateNewGenericFormatTest);
                //only authenticated users can create new
            return group;
        }
        private async static Task<IResult> CreateNewGenericFormatTest() {
            return CustomResults.ErrorResponse(Err.ErrFactory.NotImplemented());
        }
    }
}
