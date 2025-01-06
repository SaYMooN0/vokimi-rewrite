using AuthenticationService.Application.Common.interfaces;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;

namespace AuthenticationService.Infrastructure.Persistence.repositories;

internal class PasswordResetRequestsRepository : IPasswordUpdateRequestsRepository
{
    public Task<ErrOrNothing> AddPasswordUpdateRequest(AppUserId appUserId, string email) {
        throw new NotImplementedException();
    }
}
