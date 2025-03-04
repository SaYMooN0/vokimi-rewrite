using ApiShared;
using ApiShared.extensions;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using TestCreationService.Api.Extensions;
using TestCreationService.Application.Common.interfaces.repositories;
using TestCreationService.Domain.GeneralTestQuestionAggregate;

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
        TestId testRouteId = context.HttpContext.GetTestIdFromRoute();
        if (question.TestId != testRouteId) {
            return CustomResults.ErrorResponse(Err.ErrFactory.NotFound(
                message: "Incorrect question access request",
                details: $"Question with id {questionId} is not in the test with id {testRouteId}"
            ));
        }
        return await next(context);
    }
}
