using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestTakingService.Domain.TestFeedbackRecordAggregate;
using TestTakingService.Domain.TestFeedbackRecordAggregate.general_test;

namespace TestTakingService.Infrastructure.Persistence.configurations.entities_configurations.test_feedback_records;

public class GeneralTestFeedbackRecordsConfigurations: IEntityTypeConfiguration<GeneralTestFeedbackRecord>
{
    public void Configure(EntityTypeBuilder<GeneralTestFeedbackRecord> builder) {
        builder.HasBaseType<BaseTestFeedbackRecord>();
        builder.ToTable("GeneralTestFeedbackRecords");

    }
}