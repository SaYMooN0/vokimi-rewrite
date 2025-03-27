using InfrastructureConfigurationShared.ValueConverters.general_format_test;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Common.general_test_questions;
using SharedKernel.Common.general_test_questions.answer_type_specific_data;
using SharedKernel.Common.tests.general_format;

namespace InfrastructureConfigurationShared.Extensions.property_builder;

public static class PropertyBuilderExtensionsForGeneralTest
{
    public static PropertyBuilder<GeneralTestFeedbackOption> HasGeneralTestFeedbackOptionConverter(
        this PropertyBuilder<GeneralTestFeedbackOption> builder
    ) => builder.HasConversion(new GeneralTestFeedbackOptionConverter());

    public static PropertyBuilder<GeneralTestAnswerTypeSpecificData> HasGeneralTestAnswerSpecificDataConversion(
        this PropertyBuilder<GeneralTestAnswerTypeSpecificData> builder
    ) {
        return builder
            .HasConversion(new GeneralTestAnswerSpecificDataConverter())
            .HasMaxLength(2040);
    }
    public static PropertyBuilder<GeneralTestQuestionTimeLimitOption> HasGeneralTestQuestionTimeLimitOptionConverter(
        this PropertyBuilder<GeneralTestQuestionTimeLimitOption> builder
    ) {
        return builder
            .HasConversion(new GeneralTestQuestionTimeLimitOptionConverter())
            .HasMaxLength(20);
    }

    public static PropertyBuilder<GeneralTestQuestionAnswersCountLimit>
        HasGeneralTestQuestionAnswersCountLimitConverter(
            this PropertyBuilder<GeneralTestQuestionAnswersCountLimit> builder
        ) {
        return builder
            .HasConversion(new GeneralTestQuestionAnswersCountLimitConverter())
            .HasMaxLength(20);
    }
}