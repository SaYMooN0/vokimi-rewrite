using InfrastructureConfigurationShared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Common.tests;
using SharedKernel.Common.tests.formats_shared.interaction_access_settings;
using TestCreationService.Domain.TestAggregate;
using TestCreationService.Domain.TestAggregate.formats_shared;
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
            .HasForeignKey("TestId");

        builder
            .Property(s => s.AllowRatings)
            .HasResourceAvailabilitySettingConversion();
        builder
            .Property(s => s.AllowComments)
            .HasResourceAvailabilitySettingConversion();
        builder
            .Property(s => s.AllowTagsSuggestions)
            .HasResourceAvailabilitySettingConversion();
    }
}