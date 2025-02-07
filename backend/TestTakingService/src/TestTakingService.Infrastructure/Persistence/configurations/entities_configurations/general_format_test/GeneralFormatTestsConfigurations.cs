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
            .HasForeignKey("_testId");

        builder
            .HasMany<GeneralTestQuestion>("_questions")
            .WithOne()
            .HasForeignKey("_testId");

        builder
            .Property<GeneralTestFeedbackOption>("_feedbackOption")
            .HasGeneralTestFeedbackOptionConverter()
            .HasColumnName("Feedback");
    }
}