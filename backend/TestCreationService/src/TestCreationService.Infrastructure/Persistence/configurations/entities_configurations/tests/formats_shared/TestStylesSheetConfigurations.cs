using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TestCreationService.Domain.TestAggregate;
using SharedKernel.Common.tests.test_styles;
using InfrastructureConfigurationShared.Extensions;
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