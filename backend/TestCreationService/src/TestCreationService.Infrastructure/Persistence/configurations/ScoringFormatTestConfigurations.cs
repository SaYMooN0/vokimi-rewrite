using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TestCreationService.Domain.TestAggregate;
using TestCreationService.Domain.TestAggregate.scoring_format;

namespace TestCreationService.Infrastructure.Persistence.configurations;

internal class ScoringFormatTestConfigurations : IEntityTypeConfiguration<ScoringFormatTest>
{
    public void Configure(EntityTypeBuilder<ScoringFormatTest> builder) {
        builder.ToTable("ScoringFormatTests");
        builder.HasBaseType<BaseTest>();
    }
}