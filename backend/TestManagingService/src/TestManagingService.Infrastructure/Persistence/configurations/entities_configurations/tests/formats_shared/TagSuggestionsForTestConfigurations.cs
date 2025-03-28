using InfrastructureConfigurationShared.Extensions.property_builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestManagingService.Domain.TestAggregate.formats_shared;

namespace TestManagingService.Infrastructure.Persistence.configurations.entities_configurations.tests.formats_shared;

public class TagSuggestionsForTestConfigurations : IEntityTypeConfiguration<TagSuggestionForTest>
{
    public void Configure(EntityTypeBuilder<TagSuggestionForTest> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasEntityIdConversion();

        builder
            .Property(x => x.Tag)
            .ValueGeneratedNever()
            .HasTestTagIdConversion();
    }
}