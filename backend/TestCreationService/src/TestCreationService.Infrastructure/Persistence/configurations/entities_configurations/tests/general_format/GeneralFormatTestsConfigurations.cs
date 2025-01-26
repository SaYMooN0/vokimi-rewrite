using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TestCreationService.Domain.TestAggregate.general_format;
using TestCreationService.Domain.TestAggregate;
using SharedKernel.Common.EntityIds;
using TestCreationService.Infrastructure.Persistence.configurations.extension;
using TestCreationService.Domain.Common;
using TestCreationService.Domain.GeneralTestQuestionAggregate;

namespace TestCreationService.Infrastructure.Persistence.configurations.entities_configurations.tests.general_format;

internal class GeneralFormatTestsConfigurations : IEntityTypeConfiguration<GeneralFormatTest>
{
    public void Configure(EntityTypeBuilder<GeneralFormatTest> builder) {
        builder.ToTable("GeneralFormatTests");
        builder.HasBaseType<BaseTest>();

        builder.OwnsOne<EntitiesOrderController<GeneralTestQuestionId>>("_questionsList",
            controller => {
                controller
                    .Property(p => p.IsShuffled)
                    .HasColumnName("ShuffleQuestions");
                controller
                    .Property<Dictionary<GeneralTestQuestionId, ushort>>("_entityOrders")
                    .HasColumnName("QuestionsOrder")
                    .HasEntityIdsOrderDictionaryConversion();
            }
        );

        builder
            .Ignore(x => x.Results);
        builder.HasMany<GeneralTestResult>("_results")
            .WithOne()
            .HasForeignKey(q => q.TestId);

        builder
            .HasOne<GeneralTestTakingProcessSettings>("TestTakingProcessSettings")
            .WithOne()
            .HasForeignKey<GeneralTestTakingProcessSettings>();


    }
}