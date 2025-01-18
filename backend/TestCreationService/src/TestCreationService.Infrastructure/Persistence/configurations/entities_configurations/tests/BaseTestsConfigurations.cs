using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TestCreationService.Domain.TestAggregate;
using TestCreationService.Infrastructure.Persistence.configurations.extension;
using SharedKernel.Common.EntityIds;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TestCreationService.Infrastructure.Persistence.configurations.value_converters;

namespace TestCreationService.Infrastructure.Persistence.configurations.entities_configurations.tests;

internal class BaseTestsConfigurations : IEntityTypeConfiguration<BaseTest>
{
    public void Configure(EntityTypeBuilder<BaseTest> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasEntityIdConversion();
        builder
            .Property("CreatorId")
            .HasConversion(new EntityIdConverter<AppUserId>());

        builder.Ignore(x => x.EditorIds);
        builder
            .Property<HashSet<AppUserId>>("_editorIds")
            .HasColumnName("EditorIds")
            .HasEntityIdsHashSetConversion();

        builder.OwnsOne(x => x.MainInfo,
            mi => {
                mi.Property(p => p.Name).HasColumnName("maininfo_Name");
                mi.Property(p => p.CoverImg).HasColumnName("maininfo_CoverImg");
                mi.Property(p => p.Description).HasColumnName("maininfo_Description");
                mi.Property(p => p.Language).HasColumnName("maininfo_Language");
            }
        );
        builder.OwnsOne(x => x.Settings, s => {
            s.Property(s => s.TestAccess).HasColumnName("settings_TestAccess");

            s.Property(s => s.AllowRatings)
                .HasResourceAvailabilitySettingConversion()
                .HasColumnName("settings_AllowRatings");

            s.Property(s => s.AllowDiscussions)
                .HasResourceAvailabilitySettingConversion()
                .HasColumnName("settings_AllowDiscussions");

            s.Property(s => s.AllowTestTakenPosts).HasColumnName("settings_AllowTestTakenPosts");

            s.Property(s => s.AllowTagsSuggestions)
                .HasResourceAvailabilitySettingConversion()
                .HasColumnName("settings_AllowTagsSuggestions");
        });
        builder.OwnsOne(x => x.Styles, s => {

        });
    }
}
