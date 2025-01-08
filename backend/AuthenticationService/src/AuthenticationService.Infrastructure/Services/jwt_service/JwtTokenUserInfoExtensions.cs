using AuthenticationService.Application.Common.models;
using AuthenticationService.Domain.Common.value_objects;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using System.Security.Claims;

namespace AuthenticationService.Infrastructure.Services.jwt_service;

internal static class JwtTokenUserInfoExtensions
{
    private const string UserIdClaimType = "user_id";
    private const string EmailClaimType = "email";

    public static List<Claim> ToClaims(this JwtTokenUserInfo tokenInfo) => [
        new Claim(UserIdClaimType, tokenInfo.UserId.Value.ToString()),
        new Claim(EmailClaimType, tokenInfo.Email.ToString())
    ];


    public static ErrOr<JwtTokenUserInfo> UserInfoFromClaims(IEnumerable<Claim> claims) {
        var claimsList = claims.ToList();

        var userIdClaim = claimsList.FirstOrDefault(c => c.Type == UserIdClaimType)?.Value ?? string.Empty;
        var emailClaim = claimsList.FirstOrDefault(c => c.Type == EmailClaimType)?.Value ?? string.Empty;

        if (!Guid.TryParse(userIdClaim, out var userGuid)) {
            return new Err("Invalid user id in the authentication token");
        }
        var emailCreationRes = Email.Create(emailClaim);
        if (emailCreationRes.IsErr()) {
            return new Err("Invalid user email in the authentication token");
        }
        return new JwtTokenUserInfo(
            userId: new(userGuid),
            email: emailCreationRes.GetSuccess()
        );
    }
}