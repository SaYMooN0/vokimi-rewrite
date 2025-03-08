using MediatR;
using SharedKernel.Common.domain.entity;
using TestManagingService.Application.Common.interfaces.repositories.feedback_records;
using TestManagingService.Domain.TestFeedbackRecordAggregate.general_test;

namespace TestManagingService.Application.TestFeedbackRecords.commands;

public record ListFeedbackForGeneralTestCommand(
    TestId TestId
) : IRequest<GeneralTestFeedbackRecord[]>;

internal class ListFeedbackForGeneralTestCommandHandler
    : IRequestHandler<ListFeedbackForGeneralTestCommand, GeneralTestFeedbackRecord[]>
{
    private readonly IGeneralTestFeedbackRecordsRepository _generalTestFeedbackRecordsRepository;

    public ListFeedbackForGeneralTestCommandHandler(
        IGeneralTestFeedbackRecordsRepository generalTestFeedbackRecordsRepository
    ) {
        _generalTestFeedbackRecordsRepository = generalTestFeedbackRecordsRepository;
    }


    public Task<GeneralTestFeedbackRecord[]> Handle(
        ListFeedbackForGeneralTestCommand request,
        CancellationToken cancellationToken
    ) => _generalTestFeedbackRecordsRepository.ListForTestAsNoTracking(request.TestId);
}