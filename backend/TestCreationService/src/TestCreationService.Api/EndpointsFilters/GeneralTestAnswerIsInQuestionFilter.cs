using ApiShared.extensions;
using ApiShared;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;
using TestCreationService.Api.Extensions;
using TestCreationService.Application.Common.interfaces.repositories.general_format_tests;
using TestCreationService.Domain.TestAggregate.general_format;

namespace TestCreationService.Api.EndpointsFilters;

public class GeneralTestAnswerIsInQuestionFilter : IEndpointFilter
{
    private readonly IGeneralTestAnswersRepository _generalTestAnswersRepository;
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next) {
        GeneralTestAnswerId answerId = context.HttpContext.GetGeneralTestAnswerIdFromRoute();
        GeneralTestAnswer? answer = await _generalTestAnswersRepository.GetById(answerId);
        if (answer is null) {
            return CustomResults.ErrorResponse(Err.ErrFactory.NotFound(
                message: "Unable to find the answer",
                details: $"There is not answer with id {answerId}"
            ));
        }
        GeneralTestQuestionId questionId = context.HttpContext.GetGeneralTestQuestionIdFromRoute();
        if (answer.QuestionId != questionId) {
            return CustomResults.ErrorResponse(Err.ErrFactory.NotFound(
                message: "Unable to find the answer",
                details: $"There is not answer with id {answerId} for the question with id {questionId}"
            ));
        }
        return await next(context);
    }
}
