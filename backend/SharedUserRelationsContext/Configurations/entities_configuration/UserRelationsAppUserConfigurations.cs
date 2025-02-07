using InfrastructureConfigurationShared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedUserRelationsContext.Entities;

namespace SharedUserRelationsContext.Configurations.entities_configuration;

internal class UserRelationsAppUserConfigurations : IEntityTypeConfiguration<UserRelationsAppUser>
{
    public void Configure(EntityTypeBuilder<UserRelationsAppUser> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasEntityIdConversion();

        builder
            .HasMany(x => x.Followings)
            .WithOne()
            .HasForeignKey(x => x.FollowerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(x => x.Followers)
            .WithOne()
            .HasForeignKey(x => x.FollowedUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}