using InfrastructureConfigurationShared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestTakingService.Domain.TestTakenRecordAggregate;

namespace TestTakingService.Infrastructure.Persistence.configurations.entities_configurations.test_taken_records;

public class BaseTestTakenRecordsConfigurations : IEntityTypeConfiguration<BaseTestTakenRecord>
{
    public void Configure(EntityTypeBuilder<BaseTestTakenRecord> builder) {
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