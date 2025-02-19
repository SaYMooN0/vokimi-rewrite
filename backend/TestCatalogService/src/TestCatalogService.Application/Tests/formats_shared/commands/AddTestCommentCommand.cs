using MediatR;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.interfaces;
using TestCatalogService.Domain.Common.interfaces.repositories;
using TestCatalogService.Domain.Common.interfaces.repositories.tests;
using TestCatalogService.Domain.TestAggregate;
using TestCatalogService.Domain.TestCommentAggregate;

namespace TestCatalogService.Application.Tests.formats_shared.commands;

public record class AddTestCommentCommand(
    TestId TestId,
    AppUserId AuthorId,
    string Text,
    TestCommentAttachment? Attachment,
    bool MarkedAsSpoiler
) : IRequest<ErrOr<TestComment>>;

public class NewTestCommentCommandHandler : IRequestHandler<AddTestCommentCommand, ErrOr<TestComment>>
{
    private readonly IBaseTestsRepository _baseTestsRepository;
    private readonly ITestCommentsRepository _testCommentsRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public NewTestCommentCommandHandler(
        IBaseTestsRepository baseTestsRepository,
        ITestCommentsRepository testCommentsRepository,
        IDateTimeProvider dateTimeProvider
    ) {
        _baseTestsRepository = baseTestsRepository;
        _testCommentsRepository = testCommentsRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ErrOr<TestComment>> Handle(AddTestCommentCommand request, CancellationToken cancellationToken) {
        BaseTest? test = await _baseTestsRepository.GetById(request.TestId);
        if (test is null) {
            return Err.ErrPresets.TestNotFound(request.TestId);
        }

        var res = await test.AddComment(
            request.AuthorId,
            request.Text,
            request.Attachment,
            request.MarkedAsSpoiler,
            _testCommentsRepository,
            _dateTimeProvider
        );
        if (res.IsErr(out var err)) {
            return err;
        }

        await _baseTestsRepository.Update(test);
        return res.GetSuccess();
    }
}