using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InfrastructureConfigurationShared.Extensions;
using TestCatalogService.Domain.AppUserAggregate;

namespace TestCatalogService.Infrastructure.Persistence.configurations.entities_configurations;

internal class AppUserConfigurations : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasEntityIdConversion();
    }
}