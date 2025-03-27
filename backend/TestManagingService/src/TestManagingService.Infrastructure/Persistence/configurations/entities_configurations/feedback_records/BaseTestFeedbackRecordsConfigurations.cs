using InfrastructureConfigurationShared.Extensions;
using InfrastructureConfigurationShared.Extensions.property_builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestManagingService.Domain.FeedbackRecordAggregate;

namespace TestManagingService.Infrastructure.Persistence.configurations.entities_configurations.feedback_records;

public class BaseTestFeedbackRecordsConfigurations: IEntityTypeConfiguration<BaseTestFeedbackRecord>
{
    public void Configure(EntityTypeBuilder<BaseTestFeedbackRecord> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasEntityIdConversion();

        builder
            .Property(x => x.UserId)
            .ValueGeneratedNever()
            .HasNullableEntityIdConversion();
        
        builder
            .Property(x => x.TestId)
            .ValueGeneratedNever()
            .HasEntityIdConversion();
    }
}