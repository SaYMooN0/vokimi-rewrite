using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TestCreationService.Domain.TestAggregate.general_format;
using TestCreationService.Infrastructure.Persistence.configurations.extension;

namespace TestCreationService.Infrastructure.Persistence.configurations.entities_configurations.tests.general_format;

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
            .HasMany<GeneralTestAnswer>("_answers")
            .WithOne()
            .HasForeignKey(q => q.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Property(x => x.TimeLimit)
            .HasTimeLimitOptionConversion();

        builder
            .Property(x=>x.AnswerCountLimit)
            .HasAnswersCountLimitConversion();

    }
}