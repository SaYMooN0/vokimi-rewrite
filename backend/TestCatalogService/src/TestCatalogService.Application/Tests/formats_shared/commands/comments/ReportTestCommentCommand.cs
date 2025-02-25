using MediatR;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.interfaces;
using TestCatalogService.Domain.Common;
using TestCatalogService.Domain.Common.interfaces.repositories.tests;
using TestCatalogService.Domain.TestAggregate;
using TestCatalogService.Domain.TestAggregate.formats_shared.comment_reports;

namespace TestCatalogService.Application.Tests.formats_shared.commands.comments;

public record ReportTestCommentCommand(
    TestId TestId,
    TestCommentId CommentId,
    AppUserId UserId,
    string ReportText,
    CommentReportReason ReportReason
) : IRequest<ErrOrNothing>;

public class ReportTestCommentCommandHandler : IRequestHandler<ReportTestCommentCommand, ErrOrNothing>
{
    private readonly IBaseTestsRepository _baseTestsRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ReportTestCommentCommandHandler(
        IBaseTestsRepository baseTestsRepository,
        IDateTimeProvider dateTimeProvider
    ) {
        _baseTestsRepository = baseTestsRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ErrOrNothing> Handle(ReportTestCommentCommand request, CancellationToken cancellationToken) {
        BaseTest? test = await _baseTestsRepository.GetById(request.TestId);
        if (test is null) {
            return Err.ErrPresets.TestNotFound(request.TestId);
        }

        var reportRes = test.ReportComment(
            request.UserId,
            request.CommentId,
            request.ReportText,
            request.ReportReason, _dateTimeProvider
        );
        if (reportRes.IsErr(out var err)) {
            return err;
        }

        await _baseTestsRepository.Update(test);
        return ErrOrNothing.Nothing;
    }
}