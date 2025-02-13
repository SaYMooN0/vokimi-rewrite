using InfrastructureConfigurationShared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Common.domain;
using TestTakingService.Domain.TestAggregate.general_format;

namespace TestTakingService.Infrastructure.Persistence.configurations.entities_configurations.tests.general_format_test;

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