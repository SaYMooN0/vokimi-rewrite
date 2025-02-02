using InfrastructureConfigurationShared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Common.domain;
using TestCreationService.Domain.GeneralTestQuestionAggregate;
using TestCreationService.Infrastructure.Persistence.configurations.extensions;
namespace TestCreationService.Infrastructure.Persistence.configurations.entities_configurations.tests.general_format;

internal class GeneralTestAnswersConfigurations : IEntityTypeConfiguration<GeneralTestAnswer>
{
    public void Configure(EntityTypeBuilder<GeneralTestAnswer> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasEntityIdConversion();

        builder
            .Property<HashSet<GeneralTestResultId>>("_relatedResultIds")
            .HasColumnName("RelatedResultIds")
            .HasEntityIdsHashSetConversion();

        builder
            .Property(x => x.TypeSpecificData)
            .HasGeneralTestAnswerSpecificDataConversion();
    }
}