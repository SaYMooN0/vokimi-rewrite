using InfrastructureConfigurationShared.ValueConverters;
using InfrastructureConfigurationShared.ValueConverters.entity_id_related;
using InfrastructureConfigurationShared.ValueConverters.general_format_test;
using InfrastructureConfigurationShared.ValueСonverters.entity_id_related;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity_id;
using TestCreationService.Infrastructure.Persistence.configurations.value_converters.general_format_test;

namespace InfrastructureConfigurationShared.Extensions;

public static class PropertyBuilderExtensions
{
    public static PropertyBuilder<TId> HasEntityIdConversion<TId>(this PropertyBuilder<TId> builder)
        where TId : EntityId {
        return builder.HasConversion(new EntityIdConverter<TId>());
    }

    public static PropertyBuilder<TId?> HasNullableEntityIdConversion<TId>(this PropertyBuilder<TId?> builder)
        where TId : EntityId {
        return builder.HasConversion(new NullableEntityIdConverter<TId>());
    }

    public static PropertyBuilder<HashSet<T>> HasEntityIdsHashSetConversion<T>(
        this PropertyBuilder<HashSet<T>> builder) where T : EntityId {
        builder.HasConversion(
            new EntityIdHashSetConverter<T>(),
            new EntityIdHashSetComparer<T>()
        );
        return builder;
    }

    public static PropertyBuilder<TProperty> HasGeneralTestFeedbackOptionConverter<TProperty>(
        this PropertyBuilder<TProperty> builder
    ) where TProperty : class {
        return builder.HasConversion(new GeneralTestFeedbackOptionConverter());
    }

    public static PropertyBuilder<TProperty> HasGeneralTestAnswerSpecificDataConversion<TProperty>(
        this PropertyBuilder<TProperty> builder
    ) where TProperty : class {
        return builder
            .HasConversion(new GeneralTestAnswerSpecificDataConverter())
            .HasMaxLength(2040);
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

    public static PropertyBuilder<TProperty> HasHexColorConversion<TProperty>(
        this PropertyBuilder<TProperty> builder
    ) where TProperty : class {
        return builder
            .HasConversion(new HexColorConverter())
            .HasMaxLength(7);
    }
}