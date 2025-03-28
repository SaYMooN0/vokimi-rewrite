using InfrastructureConfigurationShared.Extensions.property_builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestTakingService.Domain.TestAggregate;
using TestTakingService.Domain.TestAggregate.general_format;

namespace TestTakingService.Infrastructure.Persistence.configurations.entities_configurations.tests.general_format;

internal class GeneralFormatTestsConfigurations : IEntityTypeConfiguration<GeneralFormatTest>
{
    public void Configure(EntityTypeBuilder<GeneralFormatTest> builder) {
        builder.HasBaseType<BaseTest>();
        builder.ToTable("GeneralFormatTests");
        
        builder
            .HasMany<GeneralTestResult>(x => x.Results)
            .WithOne()
            .HasForeignKey("TestId");

        builder
            .HasMany(x => x.Questions)
            .WithOne()
            .HasForeignKey("TestId");

        builder
            .Property(x => x.FeedbackOption)
            .HasGeneralTestFeedbackOptionConverter()
            .HasColumnName("Feedback");
    }
}