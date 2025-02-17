using MediatR;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;

namespace TestCreationService.Application.Tests.formats_shared.commands;

public record class DeleteTestCommand(TestId TestId) : IRequest<ErrOrNothing>;
public class DeleteTestCommandHandler : IRequestHandler<DeleteTestCommand, ErrOrNothing>
{
    public async Task<ErrOrNothing> Handle(DeleteTestCommand request, CancellationToken cancellationToken) {
        return Err.ErrFactory.NotImplemented();

        //switch test format and create domain event 'format'TestDeleted
    }
}

