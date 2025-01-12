using MediatR;
using SharedKernel.Common.errors;
using SharedKernel.Common.EntityIds;

namespace TestCreationService.Application.Tests.generic_format;

public record class CreateNewGenericFormatTestCommand(string testName, AppUserId creatorId, AppUserId[] editorIds) : IRequest<ErrOr<TestId>>;


public class CreateNewGenericFormatTestCommandHandler : IRequestHandler<CreateNewGenericFormatTestCommand, ErrOr<TestId>>
{
    public async Task<ErrOr<TestId>> Handle(CreateNewGenericFormatTestCommand request, CancellationToken cancellationToken) {

        return Err.ErrFactory.NotImplemented();
    }
}
