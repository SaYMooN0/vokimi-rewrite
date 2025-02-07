using InfrastructureConfigurationShared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Common.domain;
using TestTakingService.Domain.TestAggregate.general_format;

namespace TestTakingService.Infrastructure.Persistence.configurations.entities_configurations.general_format_test;

internal class GeneralTestQuestionsConfigurations : IEntityTypeConfiguration<GeneralTestQuestion>
{
    public void Configure(EntityTypeBuilder<GeneralTestQuestion> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasEntityIdConversion();

        builder
            .Property<TestId>("_testId")
            .ValueGeneratedNever()
            .HasEntityIdConversion();

        builder
            .HasMany<GeneralTestAnswer>("_answers")
            .WithOne()
            .HasForeignKey("_questionId");

        builder
            .Property(x => x.TimeLimit)
            .HasGeneralTestQuestionTimeLimitOptionConverter();

        builder
            .Property(x => x.AnswersCountLimit)
            .HasGeneralTestQuestionAnswersCountLimitConverter();
    }
}