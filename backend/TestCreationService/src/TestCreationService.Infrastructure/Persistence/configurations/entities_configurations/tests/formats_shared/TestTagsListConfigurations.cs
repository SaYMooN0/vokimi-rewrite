
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Common.tests.test_styles;
using TestCreationService.Domain.TestAggregate.formats_shared;
using TestCreationService.Domain.TestAggregate;
using InfrastructureConfigurationShared.Extensions;

namespace TestCreationService.Infrastructure.Persistence.configurations.entities_configurations.tests.formats_shared;

internal class TestTagsListConfigurations : IEntityTypeConfiguration<TestTagsList>
{
    public void Configure(EntityTypeBuilder<TestTagsList> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasEntityIdConversion();

        builder
            .HasOne<BaseTest>()
            .WithOne("_tags")
            .HasForeignKey<TestTagsList>("_testId");

        builder
            .Property<List<string>>("_tags")
            .HasColumnName("Tags");


    }
}