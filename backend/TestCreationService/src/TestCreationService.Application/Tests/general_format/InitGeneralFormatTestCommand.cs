using MediatR;
using SharedKernel.Common.errors;
using SharedKernel.Common.EntityIds;

namespace TestCreationService.Application.Tests.general_format;

public record class InitGeneralFormatTestCommand(string testName, AppUserId creatorId, AppUserId[] editorIds) : IRequest<ErrOr<TestId>>;


public class InitGeneralFormatTestCommandHandler : IRequestHandler<InitGeneralFormatTestCommand, ErrOr<TestId>>
{
    public async Task<ErrOr<TestId>> Handle(InitGeneralFormatTestCommand request, CancellationToken cancellationToken) {

        return Err.ErrFactory.NotImplemented();
    }
}
