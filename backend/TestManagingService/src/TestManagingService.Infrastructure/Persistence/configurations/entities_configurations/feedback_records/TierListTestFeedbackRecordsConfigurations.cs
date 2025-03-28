using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestManagingService.Domain.FeedbackRecordAggregate;
using TestManagingService.Domain.FeedbackRecordAggregate.tier_list_test;

namespace TestManagingService.Infrastructure.Persistence.configurations.entities_configurations.feedback_records;

public class TierListTestFeedbackRecordsConfigurations: IEntityTypeConfiguration<TierListTestFeedbackRecord>
{
    public void Configure(EntityTypeBuilder<TierListTestFeedbackRecord> builder) {
        builder.HasBaseType<BaseTestFeedbackRecord>();
        builder.ToTable("TierListTestFeedbackRecords");

    }
}