using AuthenticationService.Domain.Common;
using AuthenticationService.Domain.UnconfirmedAppUserAggregate;
using SharedKernel.Common.errors;

namespace AuthenticationService.Application.Common.interfaces.repositories;

public interface IUnconfirmedAppUsersRepository
{
    Task<UnconfirmedAppUser?> GetByEmail(string email);
    Task<UnconfirmedAppUser?> GetById(UnconfirmedAppUserId userId);

    Task AddNew(UnconfirmedAppUser unconfirmedAppUser);
    Task OverrideExistingWithEmail(UnconfirmedAppUser unconfirmedAppUser);

    Task RemoveById(UnconfirmedAppUserId userId);

}
