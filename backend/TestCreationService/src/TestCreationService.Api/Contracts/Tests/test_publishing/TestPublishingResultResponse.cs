using TestCreationService.Domain.TestAggregate.formats_shared;

namespace TestCreationService.Api.Contracts.Tests.test_publishing;

public record class TestPublishingResultResponse(
    bool TestPublished,
    TestPublishingProblem[] Problems,
    string TestId
);