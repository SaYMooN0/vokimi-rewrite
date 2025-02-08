using InfrastructureConfigurationShared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Common.tests.general_format;
using TestTakingService.Domain.TestAggregate;
using TestTakingService.Domain.TestAggregate.general_format;

namespace TestTakingService.Infrastructure.Persistence.configurations.entities_configurations.general_format_test;

internal class GeneralFormatTestsConfigurations : IEntityTypeConfiguration<GeneralFormatTest>
{
    public void Configure(EntityTypeBuilder<GeneralFormatTest> builder) {
        builder.ToTable("GeneralFormatTests");
        builder.HasBaseType<BaseTest>();

        builder
            .HasMany<GeneralTestResult>("_results")
            .WithOne()
            .HasForeignKey();

        builder
            .HasMany(x => x.Questions)
            .WithOne()
            .HasForeignKey();

        builder
            .Property<GeneralTestFeedbackOption>("_feedbackOption")
            .HasGeneralTestFeedbackOptionConverter()
            .HasColumnName("Feedback");
    }
}