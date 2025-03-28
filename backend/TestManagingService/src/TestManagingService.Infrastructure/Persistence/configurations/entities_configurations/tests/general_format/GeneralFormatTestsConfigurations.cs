using InfrastructureConfigurationShared.Extensions.property_builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestManagingService.Domain.TestAggregate;
using TestManagingService.Domain.TestAggregate.general_format;

namespace TestManagingService.Infrastructure.Persistence.configurations.entities_configurations.tests.general_format;

internal class GeneralFormatTestsConfigurations : IEntityTypeConfiguration<GeneralFormatTest>
{
    public void Configure(EntityTypeBuilder<GeneralFormatTest> builder) {
        builder.ToTable("GeneralFormatTests");
        builder.HasBaseType<BaseTest>();

        builder
            .Property(x => x.FeedbackOption)
            .HasGeneralTestFeedbackOptionConverter();
    }
}