using ApiShared.interfaces;
using MediatR;

namespace TestCreationService.Api.Contracts.Tests.test_creation.tier_list_format.items;

internal class UpdateTierListTestItemRequest : IRequestWithValidationNeeded
{
    public RequestValidationResult Validate() { }
}