using InfrastructureConfigurationShared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestTakingService.Domain.TestFeedbackRecordAggregate;

namespace TestTakingService.Infrastructure.Persistence.configurations.entities_configurations.test_feedback_records;

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
        
        builder
            .Property(x => x.TestTakenRecordId)
            .ValueGeneratedNever()
            .HasEntityIdConversion();
    }
}