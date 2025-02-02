using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TestCreationService.Domain.TestAggregate.formats_shared;
using TestCreationService.Domain.TestAggregate;
using InfrastructureConfigurationShared.Extensions;
using TestCreationService.Infrastructure.Persistence.configurations.extensions;

namespace TestCreationService.Infrastructure.Persistence.configurations.entities_configurations.tests.formats_shared;

internal class TestInteractionsAccessSettingsConfigurations : IEntityTypeConfiguration<TestInteractionsAccessSettings>
{
    public void Configure(EntityTypeBuilder<TestInteractionsAccessSettings> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasEntityIdConversion();

        builder
            .HasOne<BaseTest>()
            .WithOne("_interactionsAccessSettings")
            .HasForeignKey<TestInteractionsAccessSettings>("TestId");

        builder
            .Property(s => s.AllowRatings)
            .HasResourceAvailabilitySettingConversion();
        builder
            .Property(s => s.AllowDiscussions)
            .HasResourceAvailabilitySettingConversion();
        builder
            .Property(s => s.AllowTagsSuggestions)
            .HasResourceAvailabilitySettingConversion();

    }
}