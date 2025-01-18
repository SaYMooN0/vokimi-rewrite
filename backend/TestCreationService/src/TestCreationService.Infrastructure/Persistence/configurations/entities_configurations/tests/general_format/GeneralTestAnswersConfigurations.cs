using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestCreationService.Domain.TestAggregate.general_format;
using TestCreationService.Infrastructure.Persistence.configurations.extension;

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
        
        //temp
        builder.Ignore("_relatedResults");
        builder.Ignore(x=>x.RelatedResults);
    }
}