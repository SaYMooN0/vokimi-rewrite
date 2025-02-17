using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;

namespace AuthenticationService.Domain.Common;

public class UnconfirmedAppUserId : EntityId
{
    public UnconfirmedAppUserId(Guid value) : base(value) { }
    public static UnconfirmedAppUserId CreateNew() => new(Guid.CreateVersion7());
}
