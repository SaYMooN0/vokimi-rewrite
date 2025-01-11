using ApiShared;
using ApiShared.extensions;
using SharedKernel.Common.errors;
using TestCreationService.Api.Contracts.Tests.generic_format;

namespace TestCreationService.Api.Endpoints.test_creation
{
    internal static class GenericFormatTestCreationHandlers
    {
        internal static RouteGroupBuilder MapGenericFormatTestCreationHandlers(this RouteGroupBuilder group) {
            group.MapPost("/new", CreateNewGenericFormatTest)
                .AuthenticationRequired()
                .WithRequestValidation<CreateNewGenericFormatTest>();
            return group;
        }
        private async static Task<IResult> CreateNewGenericFormatTest() {
            return CustomResults.ErrorResponse(Err.ErrFactory.NotImplemented());
        }
    }
}
