using InfrastructureConfigurationShared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestTakingService.Domain.TestTakenRecordAggregate;
using TestTakingService.Domain.TestTakenRecordAggregate.general_test;

namespace TestTakingService.Infrastructure.Persistence.configurations.entities_configurations.test_taken_records.general_test_taken_records;

public class GeneralTestTakenRecordsConfigurations : IEntityTypeConfiguration<GeneralTestTakenRecord>
{
    public void Configure(EntityTypeBuilder<GeneralTestTakenRecord> builder) {
        builder.HasBaseType<BaseTestTakenRecord>();
        builder.ToTable("GeneralTestTakenRecords");

        
        builder
            .Property(x => x.ReceivedResultId)
            .ValueGeneratedNever()
            .HasEntityIdConversion();
        builder.Ignore(x=>x.ReceivedResultId);

        builder
            .HasMany(x => x.QuestionDetails)
            .WithOne()
            .HasForeignKey("TestTakenRecordId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}