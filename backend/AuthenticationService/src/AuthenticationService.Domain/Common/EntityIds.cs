using SharedKernel.Common.EntityIds;

namespace AuthenticationService.Domain.Common;

public class UnconfirmedAppUserId : EntityId
{
    private UnconfirmedAppUserId(Guid value) { Value = value; }
    public static UnconfirmedAppUserId CreateNew() => new(Guid.CreateVersion7());
}
