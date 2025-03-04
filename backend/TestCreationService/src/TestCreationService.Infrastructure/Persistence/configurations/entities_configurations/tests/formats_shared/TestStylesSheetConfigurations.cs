using InfrastructureConfigurationShared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Common.tests.formats_shared.test_styles;
using TestCreationService.Domain.TestAggregate;
using TestCreationService.Infrastructure.Persistence.configurations.extensions;

namespace TestCreationService.Infrastructure.Persistence.configurations.entities_configurations.tests.formats_shared;

internal class TestStylesSheetConfigurations : IEntityTypeConfiguration<TestStylesSheet>
{
    public void Configure(EntityTypeBuilder<TestStylesSheet> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasEntityIdConversion();

        builder
            .HasOne<BaseTest>()
            .WithOne("_styles")
            .HasForeignKey<TestStylesSheet>("TestId");
        
        builder
            .Property(x=>x.AccentColor)
            .HasHexColorConversion();
        
        builder
            .Property(x => x.ErrorsColor)
            .HasHexColorConversion();

        builder.OwnsOne(x => x.Buttons,
            b => {
                b.Property(p => p.Content).HasColumnName("buttons_Content");
                b.Property(p => p.FillType).HasColumnName("buttons_FillType");
                b.Property(p => p.IconsKey).HasColumnName("buttons_IconsKey");
            }
        );
    }
}