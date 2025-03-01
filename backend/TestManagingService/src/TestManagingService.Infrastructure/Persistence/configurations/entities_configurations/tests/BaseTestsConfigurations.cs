using InfrastructureConfigurationShared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Common.domain.entity;
using TestManagingService.Domain.TestAggregate;

namespace TestManagingService.Infrastructure.Persistence.configurations.entities_configurations.tests;

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
            .Property(x => x.EditorIds)
            .HasEntityIdsImmutableArrayConversion();

        builder
            .Property(x => x.CreatorId)
            .HasEntityIdConversion();
        
       
    }
}