using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TestCreationService.Domain.TestAggregate.general_format;
using TestCreationService.Domain.TestAggregate;
using SharedKernel.Common.EntityIds;
using TestCreationService.Infrastructure.Persistence.configurations.extension;

namespace TestCreationService.Infrastructure.Persistence.configurations.entities_configurations.tests.general_format;

internal class GeneralFormatTestsConfigurations : IEntityTypeConfiguration<GeneralFormatTest>
{
    public void Configure(EntityTypeBuilder<GeneralFormatTest> builder) {
        builder.ToTable("GeneralFormatTests");
        builder.HasBaseType<BaseTest>();

        builder.HasMany<GeneralTestQuestion>("_questions")
            .WithOne()
            .HasForeignKey(q => q.TestId)
            .OnDelete(DeleteBehavior.Cascade);


        builder
            .Property<Dictionary<GeneralTestQuestionId, ushort>>("_questionsOrderDictionary")
            .HasColumnName("QuestionsOrder")
            .HasEntityIdsOrderDictionaryConversion();

        builder.HasMany<GeneralTestResult>("_results")
            .WithOne()
            .HasForeignKey(q => q.TestId)
            .OnDelete(DeleteBehavior.Cascade);


    }
}