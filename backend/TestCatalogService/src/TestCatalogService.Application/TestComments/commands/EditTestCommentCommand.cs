using MediatR;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.interfaces;
using TestCatalogService.Domain.Common;
using TestCatalogService.Domain.Common.interfaces.repositories;
using TestCatalogService.Domain.TestCommentAggregate;

namespace TestCatalogService.Application.TestComments.commands;

public record class EditTestCommentCommand(
    TestCommentId CommentId,
    AppUserId EditingUserId,
    string NewText
) : IRequest<ErrOr<TestComment>>;

public class EditTestCommentCommandHandler : IRequestHandler<EditTestCommentCommand, ErrOr<TestComment>>
{
    private readonly ITestCommentsRepository _testCommentsRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public EditTestCommentCommandHandler(
        ITestCommentsRepository testCommentsRepository,
        IDateTimeProvider dateTimeProvider
    ) {
        _testCommentsRepository = testCommentsRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ErrOr<TestComment>> Handle(
        EditTestCommentCommand request, CancellationToken cancellationToken
    ) {
        TestComment? comment = await _testCommentsRepository.GetById(request.CommentId);
        if (comment is null) {
            return TestCatalogErrPresets.CommentNotFound(request.CommentId);
        }

        var editingRes = comment.Edit(request.EditingUserId, request.NewText, _dateTimeProvider);
        if (editingRes.IsErr(out var err)) {
            return err;
        }

        await _testCommentsRepository.Update(comment);
        return comment;
    }
}