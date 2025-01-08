using AuthenticationService.Domain.Common.value_objects;
using SharedKernel.Common.EntityIds;

namespace AuthenticationService.Application.Common.models;

public class JwtTokenUserInfo
{
    public AppUserId UserId { get; init; }
    public Email Email { get; init; }
    public JwtTokenUserInfo(AppUserId userId, Email email) {
        UserId = userId;
        Email = email;
    }
}
