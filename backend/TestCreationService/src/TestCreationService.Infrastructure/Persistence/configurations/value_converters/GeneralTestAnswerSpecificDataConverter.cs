using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.tests.general_format_tests;

namespace TestCreationService.Infrastructure.Persistence.configurations.value_converters;

//internal class GeneralTestAnswerSpecificDataConverter: ValueConverter<GeneralTestAnswerTypeSpecificData, string> 
//{
//    public GeneralTestAnswerSpecificDataConverter()
//        : base(){ } //json serialization and deserialization with derived types
//}
//internal class GeneralTestAnswerSpecificDataComparer<T> : ValueComparer<GeneralTestAnswerTypeSpecificData> 
//{
//    public GeneralTestAnswerSpecificDataComparer() : base() { } //.matchingenumtype and if true compar by hash
//}