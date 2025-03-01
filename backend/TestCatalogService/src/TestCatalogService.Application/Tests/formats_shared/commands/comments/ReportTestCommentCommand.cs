using MediatR;
using SharedKernel.Common.common_enums;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.interfaces;
using TestCatalogService.Domain.Common.interfaces.repositories;
using TestCatalogService.Domain.Common.interfaces.repositories.tests;
using TestCatalogService.Domain.TestAggregate;

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
    private readonly ITestCommentsRepository _testCommentsRepository;
    private readonly IAppUsersRepository _appUsersRepository;

    public ReportTestCommentCommandHandler(
        IBaseTestsRepository baseTestsRepository,
        ITestCommentsRepository testCommentsRepository,
        IAppUsersRepository appUsersRepository
    ) {
        _baseTestsRepository = baseTestsRepository;
        _testCommentsRepository = testCommentsRepository;
        _appUsersRepository = appUsersRepository;
    }

    public async Task<ErrOrNothing> Handle(ReportTestCommentCommand request, CancellationToken cancellationToken) {
        BaseTest? test = await _baseTestsRepository.GetById(request.TestId);
        if (test is null) {
            return Err.ErrPresets.TestNotFound(request.TestId);
        }

        ErrOrNothing reportRes = await test.ReportComment(
            request.UserId,
            request.CommentId,
            request.ReportText,
            request.ReportReason,
            _testCommentsRepository,
            _appUsersRepository
        );
        
        if (reportRes.IsErr(out var err)) {
            return err;
        }

        await _baseTestsRepository.Update(test);
        return ErrOrNothing.Nothing;
    }
}