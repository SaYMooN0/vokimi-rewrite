using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TestCreationService.Domain.TestAggregate.general_format;
using TestCreationService.Infrastructure.Persistence.configurations.extension;


namespace TestCreationService.Infrastructure.Persistence.configurations.entities_configurations.tests.general_format;

internal class GeneralTestTakingProcessSettingsConfigurations : IEntityTypeConfiguration<GeneralTestTakingProcessSettings>
{
    public void Configure(EntityTypeBuilder<GeneralTestTakingProcessSettings> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasEntityIdConversion();
        
        builder
            .HasOne<GeneralFormatTest>()
            .WithOne("TestTakingProcessSettings")
            .HasForeignKey<GeneralTestTakingProcessSettings>("TestId");

        builder
            .Property(x => x.Feedback)
            .HasTestFeedbackOptionConverter();

    }
}