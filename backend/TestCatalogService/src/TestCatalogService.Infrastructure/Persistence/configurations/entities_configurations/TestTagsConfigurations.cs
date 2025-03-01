using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Common.domain.entity;
using TestCatalogService.Domain.TestTagAggregate;

namespace TestCatalogService.Infrastructure.Persistence.configurations.entities_configurations;

internal class TestTagsConfigurations : IEntityTypeConfiguration<TestTag>
{
    public void Configure(EntityTypeBuilder<TestTag> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,           
                value => new TestTagId(value)  
            );
    }
}