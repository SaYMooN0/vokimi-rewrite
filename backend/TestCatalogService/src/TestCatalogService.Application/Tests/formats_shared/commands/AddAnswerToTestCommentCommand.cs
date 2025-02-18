using MediatR;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.interfaces;
using TestCatalogService.Application.Common.interfaces.repositories;
using TestCatalogService.Application.Common.interfaces.repositories.tests;
using TestCatalogService.Domain.Common;
using TestCatalogService.Domain.TestAggregate;
using TestCatalogService.Domain.TestCommentAggregate;

namespace TestCatalogService.Application.Tests.formats_shared.commands;

public record class AddAnswerToTestCommentCommand(
    TestCommentId ParentCommentId,
    TestId TestId,
    AppUserId AuthorId,
    string Text,
    TestCommentAttachment? Attachment,
    bool MarkedAsSpoiler
) : IRequest<ErrOr<TestComment>>;

public class AddAnswerToTestCommentCommandHandler : IRequestHandler<AddAnswerToTestCommentCommand, ErrOr<TestComment>>
{
    private readonly IBaseTestsRepository _baseTestsRepository;
    private readonly ITestCommentsRepository _testCommentsRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public async Task<ErrOr<TestComment>> Handle(AddAnswerToTestCommentCommand request,
        CancellationToken cancellationToken) {
        BaseTest? test = await _baseTestsRepository.GetById(request.TestId);
        if (test is null) {
            return Err.ErrPresets.TestNotFound(request.TestId);
        }

        return await test.AddAnswerToComment(
            request.ParentCommentId,
            request.AuthorId,
            request.Text,
            request.Attachment,
            request.MarkedAsSpoiler,
            _testCommentsRepository,
            _dateTimeProvider
        );
    }
}