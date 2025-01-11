using SharedKernel.Common;
using SharedKernel.Common.EntityIds;

namespace AuthenticationService.Domain.Events;

public record NewAppUserCreated(AppUserId CreateUserId) : IDomainEvent;
