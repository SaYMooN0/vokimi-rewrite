using InfrastructureConfigurationShared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestCatalogService.Domain.TestCommentAggregate;
using TestCatalogService.Infrastructure.Persistence.configurations.value_converters;

namespace TestCatalogService.Infrastructure.Persistence.configurations.entities_configurations;

public class TestCommentsConfigurations : IEntityTypeConfiguration<TestComment>
{
    public void Configure(EntityTypeBuilder<TestComment> builder) {
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
            .Property(x => x.Author)
            .ValueGeneratedNever()
            .HasEntityIdConversion();
        
        builder
            .Property(x => x.Attachment)
            .HasConversion(new TestCommentAttachmentConverter());

        builder.Ignore(x => x.Answers);
        builder
            .HasMany<TestComment>("_answers")
            .WithOne(x => x.ParentComment)
            .HasForeignKey("ParentCommentId");
    }
}