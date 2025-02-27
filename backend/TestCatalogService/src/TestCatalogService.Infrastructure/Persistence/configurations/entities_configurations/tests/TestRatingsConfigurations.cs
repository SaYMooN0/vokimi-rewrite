using InfrastructureConfigurationShared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestCatalogService.Domain.TestAggregate.formats_shared.ratings;

namespace TestCatalogService.Infrastructure.Persistence.configurations.entities_configurations.tests;

internal class TestRatingsConfigurations : IEntityTypeConfiguration<TestRating>
{
    public void Configure(EntityTypeBuilder<TestRating> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasEntityIdConversion();
        
        builder
            .Property(x => x.UserId)
            .ValueGeneratedNever()
            .HasEntityIdConversion();
    }
}