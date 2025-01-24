using ApiShared.extensions;
using ApiShared;
using MediatR;
using SharedKernel.Common.EntityIds;
using TestCreationService.Api.Contracts.Tests.test_creation.general_format.questions;
using TestCreationService.Api.Extensions;
using TestCreationService.Application.Tests.general_format.commands.questions;

namespace TestCreationService.Api.Endpoints.test_creation.general;
internal static class GeneralTestCreationAnswerOperationsHandlers
{
    internal static RouteGroupBuilder MapGeneralTestCreationAnswerOperationsHandlers(this RouteGroupBuilder group) {
        group
            .GroupAuthenticationRequired()
            .GroupTestEditPermissionRequired()
            .GroupCheckIfGeneralTestQuestionInProvidedTest()
            .GroupCheckIfGeneralTestAnswerInProvidedQuestion();

        return group;
    }
}
