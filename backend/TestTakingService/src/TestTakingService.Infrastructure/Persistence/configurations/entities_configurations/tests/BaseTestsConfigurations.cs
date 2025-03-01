using InfrastructureConfigurationShared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;
using TestTakingService.Domain.Common;
using TestTakingService.Domain.TestAggregate;

namespace TestTakingService.Infrastructure.Persistence.configurations.entities_configurations.tests;

internal class BaseTestsConfigurations : IEntityTypeConfiguration<BaseTest>
{
    public void Configure(EntityTypeBuilder<BaseTest> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasEntityIdConversion();

        builder
            .Property<AppUserId>("_creatorId")
            .HasColumnName("CreatorId")
            .ValueGeneratedNever()
            .HasEntityIdConversion();
        
        builder.Ignore(x => x.TakenByUserIds);
        builder
            .Property<HashSet<AppUserId>>("_takenByUserIds")
            .HasColumnName("TakenByUserIds")
            .HasEntityIdsHashSetConversion();
        
        builder.Ignore(x => x.TestTakenRecordIds);
        builder
            .Property<HashSet<TestTakenRecordId>>("_testTakenRecordIds")
            .HasColumnName("TestTakenRecordIds")
            .HasEntityIdsHashSetConversion();
    }
}