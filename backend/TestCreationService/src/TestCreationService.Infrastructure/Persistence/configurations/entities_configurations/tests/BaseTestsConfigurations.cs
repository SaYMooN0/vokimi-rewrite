using InfrastructureConfigurationShared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;
using TestCreationService.Domain.TestAggregate;
using TestCreationService.Domain.TestAggregate.formats_shared;
using TestCreationService.Infrastructure.Persistence.configurations.extensions;
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

        builder.Ignore(x => x.EditorIds);
        builder
            .Property<HashSet<AppUserId>>("_editorIds")
            .HasColumnName("EditorIds")
            .HasEntityIdsHashSetConversion();

        builder
            .Property<AppUserId>("CreatorId")
            .HasEntityIdConversion();

        builder.OwnsOne<TestMainInfo>("_mainInfo",
            mi => {
                mi.Property(p => p.Name).HasColumnName("maininfo_Name");
                mi.Property(p => p.CoverImg).HasColumnName("maininfo_CoverImg");
                mi.Property(p => p.Description).HasColumnName("maininfo_Description");
                mi.Property(p => p.Language).HasColumnName("maininfo_Language");
            }
        );
    }
}
