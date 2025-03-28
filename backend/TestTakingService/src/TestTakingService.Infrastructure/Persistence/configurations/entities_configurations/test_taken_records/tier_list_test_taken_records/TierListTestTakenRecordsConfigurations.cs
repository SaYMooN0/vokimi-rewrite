using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestTakingService.Domain.TestTakenRecordAggregate;
using TestTakingService.Domain.TestTakenRecordAggregate.tier_list_test;

namespace TestTakingService.Infrastructure.Persistence.configurations.entities_configurations.test_taken_records.tier_list_test_taken_records;

public class TierListTestTakenRecordsConfigurations : IEntityTypeConfiguration<TierListTestTakenRecord>
{
    public void Configure(EntityTypeBuilder<TierListTestTakenRecord> builder) {
        builder.HasBaseType<BaseTestTakenRecord>();
        builder.ToTable("TierListTestTakenRecords");
        
        builder
            .HasMany(x => x.TierDetails)
            .WithOne()
            .HasForeignKey("TestTakenRecordId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}