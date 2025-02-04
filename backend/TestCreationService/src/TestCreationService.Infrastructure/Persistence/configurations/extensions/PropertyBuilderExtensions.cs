using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Common.domain;
using TestCreationService.Infrastructure.Persistence.configurations.value_converters;
using TestCreationService.Infrastructure.Persistence.configurations.value_converters.general_format_test;

namespace TestCreationService.Infrastructure.Persistence.configurations.extensions;

internal static class PropertyBuilderExtensions
{
    public static PropertyBuilder<TProperty> HasResourceAvailabilitySettingConversion<TProperty>(
        this PropertyBuilder<TProperty> builder
    ) where TProperty : class {
        return builder
            .HasConversion(new ResourceAvailabilitySettingConverter())
            .HasMaxLength(20);
    }

    public static PropertyBuilder<Dictionary<T, ushort>> HasEntityIdsOrderDictionaryConversion<T>(
        this PropertyBuilder<Dictionary<T, ushort>> builder) where T : EntityId {
        builder.HasConversion(
            new EntityIdsOrderDictionaryConverter<T>(),
            new EntityIdsOrderDictionaryComparer<T>()
        );
        return builder;
    }

    public static PropertyBuilder<TProperty> HasGeneralTestQuestionTimeLimitOptionConverter<TProperty>(
        this PropertyBuilder<TProperty> builder
    ) where TProperty : class {
        return builder
            .HasConversion(new GeneralTestQuestionTimeLimitOptionConverter())
            .HasMaxLength(20);
    }

    public static PropertyBuilder<TProperty> HasGeneralTestQuestionAnswersCountLimitConverter<TProperty>(
        this PropertyBuilder<TProperty> builder
    ) where TProperty : class {
        return builder
            .HasConversion(new GeneralTestQuestionAnswersCountLimitConverter())
            .HasMaxLength(20);
    }

    public static PropertyBuilder<TProperty> HasGeneralTestAnswerSpecificDataConversion<TProperty>(
        this PropertyBuilder<TProperty> builder
    ) where TProperty : class {
        return builder
            .HasConversion(new GeneralTestAnswerSpecificDataConverter())
            .HasMaxLength(2040);
    }

    public static PropertyBuilder<TProperty> HasGeneralTestFeedbackOptionConverter<TProperty>(
        this PropertyBuilder<TProperty> builder
    ) where TProperty : class {
        return builder.HasConversion(new GeneralTestFeedbackOptionConverter());
    }

    public static PropertyBuilder<TProperty> HasHexColorConversion<TProperty>(
        this PropertyBuilder<TProperty> builder
    ) where TProperty : class {
        return builder
            .HasConversion(new HexColorConverter())
            .HasMaxLength(7);
    }
}