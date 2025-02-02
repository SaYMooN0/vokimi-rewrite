using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TestCreationService.Domain.Common;
using TestCreationService.Domain.GeneralTestQuestionAggregate;
using SharedKernel.Common.domain;
using InfrastructureConfigurationShared.Extensions;
using TestCreationService.Infrastructure.Persistence.configurations.extensions;

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
            .Property(x => x.TestId)
            .ValueGeneratedNever()
            .HasEntityIdConversion();

        builder
            .Property<string[]>("_images")
            .HasColumnName("Images");

        builder
            .HasMany<GeneralTestAnswer>("_answers")
            .WithOne()
            .HasForeignKey(q => q.QuestionId);
        builder.OwnsOne<EntitiesOrderController<GeneralTestAnswerId>>("_answersOrderController",
            controller => {
                controller
                    .Property(p => p.IsShuffled)
                    .HasColumnName("ShuffleAnswers");
                controller
                    .Property<Dictionary<GeneralTestAnswerId, ushort>>("_entityOrders")
                    .HasColumnName("AnswersOrder")
                    .HasEntityIdsOrderDictionaryConversion();
            }
        );

        builder
            .Property(x => x.TimeLimit)
            .HasGeneralTestQuestionTimeLimitOptionConverter();

        builder
            .Property(x => x.AnswersCountLimit)
            .HasGeneralTestQuestionAnswersCountLimitConverter();

    }
}