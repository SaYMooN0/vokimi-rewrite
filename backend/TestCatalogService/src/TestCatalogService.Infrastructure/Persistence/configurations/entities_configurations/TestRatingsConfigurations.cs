using InfrastructureConfigurationShared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestCatalogService.Domain.TestAggregate.formats_shared;

namespace TestCatalogService.Infrastructure.Persistence.configurations.entities_configurations;

public class TestRatingsConfigurations : IEntityTypeConfiguration<TestRating>
{
    public void Configure(EntityTypeBuilder<TestRating> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasEntityIdConversion();
        
        builder
            .Property(x => x.TestId)
            .ValueGeneratedNever()
            .HasEntityIdConversion();
        
        builder
            .Property(x => x.AuthorId)
            .ValueGeneratedNever()
            .HasEntityIdConversion();
    }
}