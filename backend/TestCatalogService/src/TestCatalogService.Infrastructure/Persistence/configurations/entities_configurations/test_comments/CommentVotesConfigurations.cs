using InfrastructureConfigurationShared.Extensions;
using InfrastructureConfigurationShared.Extensions.property_builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestCatalogService.Domain.TestCommentAggregate;

namespace TestCatalogService.Infrastructure.Persistence.configurations.entities_configurations.test_comments;

public class CommentVotesConfigurations : IEntityTypeConfiguration<CommentVote>
{
    public void Configure(EntityTypeBuilder<CommentVote> builder) {
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