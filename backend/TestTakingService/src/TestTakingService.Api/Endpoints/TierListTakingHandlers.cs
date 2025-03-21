using ApiShared;
using ApiShared.extensions;
using MediatR;
using SharedKernel.Common.domain.entity;
using SharedKernel.Configs;
using TestTakingService.Api.Contracts.load_test_taking_data.general_test;
using TestTakingService.Api.Contracts.test_taken.general_test;
using TestTakingService.Api.Contracts.test_taken.tier_list_test;
using TestTakingService.Api.Extensions;
using TestTakingService.Application.Tests.general_format.commands;

namespace TestTakingService.Api.Endpoints;

internal static class TierListTakingHandlers
{
    internal static RouteGroupBuilder MapTierListTakingHandlers(this RouteGroupBuilder group) {
        group
            .GroupUserAccessToTakeTestRequired();

        group.MapGet("/loadTestTakingData",LoadTestTakingData);
        group.MapPost("/testTaken", HandleTierListTestTaken)
            .WithRequestValidation<TierListTestTakenRequest>();

        return group;
    }

   
}