using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;

namespace AuthenticationService.Application.Common.interfaces.repositories;

public interface IPasswordUpdateRequestsRepository
{
    Task<ErrOrNothing> Add(AppUserId appUserId, string email);
    //get password update request by userId
    //remove password update request by requestIds
}
