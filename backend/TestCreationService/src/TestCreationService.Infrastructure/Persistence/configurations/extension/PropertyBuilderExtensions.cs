using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Common.EntityIds;
using System.Collections.Immutable;
using TestCreationService.Infrastructure.Persistence.configurations.value_converters;

namespace TestCreationService.Infrastructure.Persistence.configurations.extension;

internal static class PropertyBuilderExtensions
{
    public static PropertyBuilder<TProperty> HasResourceAvailabilitySettingConversion<TProperty>(
        this PropertyBuilder<TProperty> builder
    ) where TProperty : class {
        return builder
            .HasConversion(new ResourceAvailabilitySettingConverter())
            .HasMaxLength(20);
    }
    public static PropertyBuilder<TId> HasEntityIdConversion<TId>(this PropertyBuilder<TId> builder) where TId : EntityId {
        return builder.HasConversion(new EntityIdConverter<TId>());
    }

    public static PropertyBuilder<HashSet<T>> HasEntityIdsHashSetConversion<T>(
        this PropertyBuilder<HashSet<T>> builder) where T : EntityId {
        builder.HasConversion(
            new EntityIdHashSetConverter<T>(),
            new HashSetOfEntityIdsComparer<T>()
        );
        return builder;
    }
    public static PropertyBuilder<Dictionary<T, ushort>> HasEntityIdsOrderDictionaryConversion<T>(
        this PropertyBuilder<Dictionary<T, ushort>> builder) where T : EntityId {
        builder.HasConversion(
            new EntityIdsOrderDictionaryConverter<T>(),
            new EntityIdsOrderDictionaryComparer<T>()
        );
        return builder;
    }
    public static PropertyBuilder<TProperty> HasTimeLimitOptionConversion<TProperty>(
        this PropertyBuilder<TProperty> builder
    ) where TProperty : class {
        return builder
            .HasConversion(new TimeLimitOptionConverter())
            .HasMaxLength(20);
    }
    public static PropertyBuilder<TProperty> HasAnswersCountLimitConversion<TProperty>(
        this PropertyBuilder<TProperty> builder
    ) where TProperty : class {
        return builder
            .HasConversion(new AnswerCountLimitConverter())
            .HasMaxLength(20);
    }
}