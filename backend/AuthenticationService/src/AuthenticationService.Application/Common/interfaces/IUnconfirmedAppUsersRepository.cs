using AuthenticationService.Domain.UnconfirmedAppUserAggregate;
using SharedKernel.Common.errors;

namespace AuthenticationService.Application.Common.interfaces;

public interface IUnconfirmedAppUsersRepository
{
    Task<ErrOrNothing> AddUnconfirmedAppUser(UnconfirmedAppUser appUser);

}
