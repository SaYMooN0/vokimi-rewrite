using InfrastructureConfigurationShared.Extensions;
using InfrastructureConfigurationShared.Extensions.property_builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestTakingService.Domain.TestAggregate.general_format;

namespace TestTakingService.Infrastructure.Persistence.configurations.entities_configurations.tests.general_format_test;

internal class GeneralTestAnswersConfigurations : IEntityTypeConfiguration<GeneralTestAnswer>
{
    public void Configure(EntityTypeBuilder<GeneralTestAnswer> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasEntityIdConversion();

        builder.Ignore(x=>x.RelatedResultIds);
        builder
            .HasMany<GeneralTestResult>("RelatedResults")
            .WithMany()
            .UsingEntity(j => j.ToTable("rel_RelatedGeneralTestResultsForAnswer"));

        builder
            .Property(x => x.TypeSpecificData)
            .HasGeneralTestAnswerSpecificDataConversion();
    }
}