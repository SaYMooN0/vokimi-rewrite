using InfrastructureConfigurationShared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestTakingService.Domain.TestTakenRecordAggregate.general_test;
using TestTakingService.Infrastructure.Persistence.configurations.value_converters;

namespace TestTakingService.Infrastructure.Persistence.configurations.entities_configurations.test_taken_records.general_test_taken_records;

public class
    GeneralTestTakenRecordQuestionDetailsConfigurations : IEntityTypeConfiguration<
    GeneralTestTakenRecordQuestionDetails>
{
    public void Configure(EntityTypeBuilder<GeneralTestTakenRecordQuestionDetails> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasEntityIdConversion();

        builder
            .Property(x => x.QuestionId)
            .ValueGeneratedNever()
            .HasEntityIdConversion();

        builder
            .Property(x => x.ChosenAnswerIds)
            .HasConversion(new GeneralTestAnswerIdArrayConverter());
    }
}