using InfrastructureConfigurationShared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestManagingService.Domain.TestAggregate.formats_shared.comment_reports;

namespace TestManagingService.Infrastructure.Persistence.configurations.entities_configurations.tests.formats_shared;

internal class TestCommentReportsConfigurations : IEntityTypeConfiguration<TestCommentReport>
{
    public void Configure(EntityTypeBuilder<TestCommentReport> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasEntityIdConversion();

        builder
            .Property(x => x.AuthorId)
            .ValueGeneratedNever()
            .HasEntityIdConversion();
        
        builder
            .Property(x => x.CommentId)
            .ValueGeneratedNever()
            .HasEntityIdConversion();
    }
}