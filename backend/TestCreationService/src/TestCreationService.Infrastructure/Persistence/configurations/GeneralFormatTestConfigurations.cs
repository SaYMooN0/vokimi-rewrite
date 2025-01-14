using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TestCreationService.Domain.TestAggregate.general_format;
using TestCreationService.Domain.TestAggregate;

namespace TestCreationService.Infrastructure.Persistence.configurations;

internal class GeneralFormatTestConfigurations : IEntityTypeConfiguration<GeneralFormatTest>
{
    public void Configure(EntityTypeBuilder<GeneralFormatTest> builder) {
        builder.ToTable("GeneralFormatTests");
        builder.HasBaseType<BaseTest>();
    }
}