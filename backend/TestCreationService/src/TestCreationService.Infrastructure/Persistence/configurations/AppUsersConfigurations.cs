using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Common.EntityIds;
using TestCreationService.Domain.AppUserAggregate;
using TestCreationService.Domain.TestAggregate;
using TestCreationService.Infrastructure.Persistence.configurations.extension;

namespace TestCreationService.Infrastructure.Persistence.configurations;

internal class AppUsersConfigurations : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder) {
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasEntityIdConversion();

        builder.Ignore(x => x.CreatedTestIds);
        builder
            .Property<HashSet<TestId>>("_createdTestIds")
            .HasColumnName("CreatedTestIds")
            .HasEntityIdsHashSetConversion();
        //builder
        //    .Property(u => u.CreatedTestIds)
        //    .HasEntityIdsHashSetConversion();

        //builder
        //    .Property(u => u.EditorAssignedTests)
        //    .HasEntityIdsHashSetConversion();

        builder.Ignore(x => x.EditorAssignedTests);
        builder
            .Property<HashSet<TestId>>("_editorAssignedTests")
            .HasColumnName("EditorAssignedTests")
            .HasEntityIdsHashSetConversion();
    }
}
