using ApiShared.extensions;
using ApiShared;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using TestCreationService.Api.Extensions;
using TestCreationService.Domain.TestAggregate.general_format;
using TestCreationService.Application.Common.interfaces.repositories.general_format_tests;

namespace TestCreationService.Api.EndpointsFilters;

internal class GeneralTestQuestionIsInTestFilter : IEndpointFilter
{
    private readonly IGeneralTestQuestionsRepository _generalTestQuestionsRepository;

    public GeneralTestQuestionIsInTestFilter(IGeneralTestQuestionsRepository generalTestQuestionsRepository) {
        _generalTestQuestionsRepository = generalTestQuestionsRepository;
    }
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next) {
        GeneralTestQuestionId questionId = context.HttpContext.GetGeneralTestQuestionIdFromRoute();
        GeneralTestQuestion? question = await _generalTestQuestionsRepository.GetById(questionId);
        if (question is null) {
            return CustomResults.ErrorResponse(Err.ErrPresets.GeneralTestQuestionNotFound(questionId));
        }
        TestId testId = context.HttpContext.GetTestIdFromRoute();
        if (question.TestId != testId) {
            return CustomResults.ErrorResponse(Err.ErrFactory.NotFound(
                message: "Unable to find the question",
                details: $"There is not question with id {questionId} in the test with id {testId}"
            ));
        }
        return await next(context);
    }
}
