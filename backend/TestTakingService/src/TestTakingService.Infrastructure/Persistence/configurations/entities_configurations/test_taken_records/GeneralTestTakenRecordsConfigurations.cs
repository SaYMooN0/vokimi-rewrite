using InfrastructureConfigurationShared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestTakingService.Domain.TestTakenRecordAggregate;
using TestTakingService.Domain.TestTakenRecordAggregate.general_test;

namespace TestTakingService.Infrastructure.Persistence.configurations.entities_configurations.test_taken_records;

public class GeneralTestTakenRecordsConfigurations: IEntityTypeConfiguration<GeneralTestTakenRecord>
{
    public void Configure(EntityTypeBuilder<GeneralTestTakenRecord> builder) {
        builder.HasBaseType<BaseTestTakenRecord>();
        
        builder
            .Property(x=>x.ReceivedResultId)
            .ValueGeneratedNever()
            .HasEntityIdConversion();
        
        builder
            .Property(x=>x.QuestionDetails)
            .HasConversion();
    }
}