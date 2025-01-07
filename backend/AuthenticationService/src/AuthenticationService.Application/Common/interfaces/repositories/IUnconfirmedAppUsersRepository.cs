using AuthenticationService.Domain.Common;
using AuthenticationService.Domain.UnconfirmedAppUserAggregate;
using SharedKernel.Common.errors;

namespace AuthenticationService.Application.Common.interfaces.repositories;

public interface IUnconfirmedAppUsersRepository
{
    Task<ErrOr<UnconfirmedAppUser>> GetByEmail(string email);
    Task<ErrOr<UnconfirmedAppUser>> GetById(UnconfirmedAppUserId userId);

    Task<ErrOrNothing> AddNew(UnconfirmedAppUser unconfirmedAppUser);
    Task<ErrOrNothing> OverrideUserWithEmail(UnconfirmedAppUser unconfirmedAppUser);
}
