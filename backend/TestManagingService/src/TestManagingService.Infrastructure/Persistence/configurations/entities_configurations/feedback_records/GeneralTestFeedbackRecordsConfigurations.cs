using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestManagingService.Domain.TestFeedbackRecordAggregate;
using TestManagingService.Domain.TestFeedbackRecordAggregate.general_test;

namespace TestManagingService.Infrastructure.Persistence.configurations.entities_configurations.feedback_records;

public class GeneralTestFeedbackRecordsConfigurations: IEntityTypeConfiguration<GeneralTestFeedbackRecord>
{
    public void Configure(EntityTypeBuilder<GeneralTestFeedbackRecord> builder) {
        builder.HasBaseType<BaseTestFeedbackRecord>();
        builder.ToTable("GeneralTestFeedbackRecords");

    }
}