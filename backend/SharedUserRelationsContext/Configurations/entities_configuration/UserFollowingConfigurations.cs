using InfrastructureConfigurationShared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedUserRelationsContext.Entities;

namespace SharedUserRelationsContext.Configurations.entities_configuration;

internal class UserFollowingConfigurations : IEntityTypeConfiguration<UserFollowing>
{
    public void Configure(EntityTypeBuilder<UserFollowing> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasEntityIdConversion();

        builder
            .Property(x => x.FollowerId)
            .HasEntityIdConversion()
            .IsRequired();
        builder
            .Property(x => x.FollowedUserId)
            .HasEntityIdConversion()
            .IsRequired();
        builder
            .Property(x => x.FollowedAt)
            .IsRequired();
        builder
            .HasIndex(x => new { x.FollowerId, x.FollowedUserId })
            .IsUnique();
    }
}