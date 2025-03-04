using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestCreationService.Domain.TestAggregate;
using TestCreationService.Domain.TestAggregate.scoring_format;

namespace TestCreationService.Infrastructure.Persistence.configurations.entities_configurations.tests.scoring_format;

internal class ScoringFormatTestsConfigurations : IEntityTypeConfiguration<ScoringFormatTest>
{
    public void Configure(EntityTypeBuilder<ScoringFormatTest> builder) {
        builder.ToTable("ScoringFormatTests");
        builder.HasBaseType<BaseTest>();
    }
}