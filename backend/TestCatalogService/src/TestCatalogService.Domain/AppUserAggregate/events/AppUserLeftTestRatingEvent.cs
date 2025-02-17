using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;
using TestCatalogService.Domain.Common;

namespace TestCatalogService.Domain.AppUserAggregate.events;

public record class AppUserLeftTestRatingEvent(AppUserId UserId, TestRatingId RatingId) : IDomainEvent;