using InfrastructureConfigurationShared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestTakingService.Domain.TestAggregate.general_format;

namespace TestTakingService.Infrastructure.Persistence.configurations.entities_configurations.general_format_test;

internal class GeneralTestAnswersConfigurations : IEntityTypeConfiguration<GeneralTestAnswer>
{
    public void Configure(EntityTypeBuilder<GeneralTestAnswer> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasEntityIdConversion();

        builder
            .HasMany<GeneralTestResult>("_relatedResults")
            .WithOne();

        builder
            .Property(x => x.TypeSpecificData)
            .HasGeneralTestAnswerSpecificDataConversion();
    }
}