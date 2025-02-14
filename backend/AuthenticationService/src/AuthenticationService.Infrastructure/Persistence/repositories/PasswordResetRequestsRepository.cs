using AuthenticationService.Application.Common.interfaces.repositories;
using SharedKernel.Common.domain;
using SharedKernel.Common.errors;

namespace AuthenticationService.Infrastructure.Persistence.repositories;

internal class PasswordResetRequestsRepository : IPasswordUpdateRequestsRepository
{
    public Task<ErrOrNothing> Add(AppUserId appUserId, string email) {
        throw new NotImplementedException();
    }
}
