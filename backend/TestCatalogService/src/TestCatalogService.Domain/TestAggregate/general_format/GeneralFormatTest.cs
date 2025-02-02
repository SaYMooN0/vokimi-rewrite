using SharedKernel.Common.domain;
using SharedKernel.Common.tests;

namespace TestCatalogService.Domain.TestAggregate.general_format;

public class GeneralFormatTest : BaseTest
{
    private GeneralFormatTest() : base() { }
    public override TestFormat Format =>TestFormat.General;
    public ushort QuestionsCount { get; init; }
    public ushort ResultsCount { get; init; }
    public bool NoAudioAnswers { get; init; }

}
