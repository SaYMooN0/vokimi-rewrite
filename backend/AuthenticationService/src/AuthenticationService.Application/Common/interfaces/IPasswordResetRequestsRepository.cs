using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;

namespace AuthenticationService.Application.Common.interfaces;

public interface IPasswordUpdateRequestsRepository
{
    Task<ErrOrNothing> AddPasswordUpdateRequest(AppUserId appUserId, string email);
    //get password update request by userId
    //remove password update request by requestIds
}
