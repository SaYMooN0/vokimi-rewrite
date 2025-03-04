using InfrastructureConfigurationShared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestCreationService.Domain.TestAggregate.general_format;

namespace TestCreationService.Infrastructure.Persistence.configurations.entities_configurations.tests.general_format;

internal class GeneralTestResultsConfigurations : IEntityTypeConfiguration<GeneralTestResult>
{
    public void Configure(EntityTypeBuilder<GeneralTestResult> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasEntityIdConversion();
    }
}