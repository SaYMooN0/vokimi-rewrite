using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Common.EntityIds;
using TestCreationService.Domain.AppUserAggregate;
using TestCreationService.Domain.TestAggregate;

namespace TestCreationService.Infrastructure.Persistence.configurations;

internal class AppUsersConfigurations : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder) {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("Id")
            .ValueGeneratedNever()
            .HasConversion(v => v.Value, v => new(v));

        builder
            .HasMany<BaseTest>()
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "TestEditors",
                j => j
                    .HasOne<BaseTest>()
                    .WithMany()
                    .HasForeignKey("BaseTestId")
                    .OnDelete(DeleteBehavior.Restrict),
                j => j
                    .HasOne<AppUser>()
                    .WithMany()
                    .HasForeignKey("AppUserId")
                    .OnDelete(DeleteBehavior.Restrict),
                j => {
                    j.Property<TestId>("BaseTestId");
                    j.Property<AppUserId>("AppUserId");
                }
            );
    }
}
