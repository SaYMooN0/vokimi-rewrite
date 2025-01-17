namespace TestCreationService.Domain.TestAggregate.general_format;

public abstract record GeneralTestAnswerTypeSpecificData
{
    public record TextOnly(string Text) : GeneralTestAnswerTypeSpecificData;
    public record ImageOnly(string Image) : GeneralTestAnswerTypeSpecificData;
    public record TextAndImage(string Text, string Image) : GeneralTestAnswerTypeSpecificData;
}
