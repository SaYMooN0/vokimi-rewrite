using System.Collections.Immutable;
using MediatR;
using SharedKernel.Common.domain.entity;
using TestManagingService.Application.Common.interfaces.repositories.feedback_records;
using TestManagingService.Domain.TestFeedbackRecordAggregate.general_test;

namespace TestManagingService.Application.TestFeedbackRecords.commands;

public record ListFilteredFeedbackForGeneralTestCommand(
    TestId TestId,
    GeneralTestFeedbackRecordsFilter Filter
) : IRequest<GeneralTestFeedbackRecord[]>;

internal class ListFilteredFeedbackForGeneralTestCommandHandler
    : IRequestHandler<ListFilteredFeedbackForGeneralTestCommand, GeneralTestFeedbackRecord[]>
{
    private readonly IGeneralTestFeedbackRecordsRepository _generalTestFeedbackRecordsRepository;

    public ListFilteredFeedbackForGeneralTestCommandHandler(
        IGeneralTestFeedbackRecordsRepository generalTestFeedbackRecordsRepository
    ) {
        _generalTestFeedbackRecordsRepository = generalTestFeedbackRecordsRepository;
    }


    public Task<GeneralTestFeedbackRecord[]> Handle(
        ListFilteredFeedbackForGeneralTestCommand request,
        CancellationToken cancellationToken
    ) => _generalTestFeedbackRecordsRepository.ListFilteredForTestAsNoTracking(request.TestId, request.Filter);
}