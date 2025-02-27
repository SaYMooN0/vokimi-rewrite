using InfrastructureConfigurationShared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestCatalogService.Domain.TestCommentAggregate;
using TestCatalogService.Infrastructure.Persistence.configurations.value_converters;

namespace TestCatalogService.Infrastructure.Persistence.configurations.entities_configurations.test_comments;

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
            .Property(x => x.AuthorId)
            .ValueGeneratedNever()
            .HasEntityIdConversion();

        builder
            .Property(x => x.ParentCommentId)
            .ValueGeneratedNever()
            .HasNullableEntityIdConversion();
        
        builder
            .Property<TestCommentAttachment?>("_attachment")
            .HasConversion(new TestCommentAttachmentConverter());

        builder.Ignore(x => x.Answers);
        builder
            .HasMany<TestComment>("_answers")
            .WithOne()
            .HasForeignKey(x => x.ParentCommentId);
        
        builder
            .HasMany<CommentVote>("_votes")
            .WithOne()
            .HasForeignKey("TestCommentId");
    }
}