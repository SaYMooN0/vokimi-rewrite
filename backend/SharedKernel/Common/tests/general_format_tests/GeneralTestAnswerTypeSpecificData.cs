namespace SharedKernel.Common.tests.general_format_tests;

public abstract record GeneralTestAnswerTypeSpecificData
{
    public abstract GeneralTestAnswersType MatchingEnumType { get; }
    public sealed record TextOnly(string Text) : GeneralTestAnswerTypeSpecificData
    { public override GeneralTestAnswersType MatchingEnumType => GeneralTestAnswersType.TextOnly; }

    public sealed record ImageOnly(string Image) : GeneralTestAnswerTypeSpecificData
    { public override GeneralTestAnswersType MatchingEnumType => GeneralTestAnswersType.ImageOnly; }

    public sealed record ImageAndText(string Text, string Image) : GeneralTestAnswerTypeSpecificData
    { public override GeneralTestAnswersType MatchingEnumType => GeneralTestAnswersType.ImageAndText; }

    public sealed record ColorOnly(string Color) : GeneralTestAnswerTypeSpecificData
    { public override GeneralTestAnswersType MatchingEnumType => GeneralTestAnswersType.ColorOnly; }

    public sealed record ColorAndText(string Color, string Text) : GeneralTestAnswerTypeSpecificData
    { public override GeneralTestAnswersType MatchingEnumType => GeneralTestAnswersType.ColorAndText; }

    public sealed record AudioOnly(string Audio) : GeneralTestAnswerTypeSpecificData
    { public override GeneralTestAnswersType MatchingEnumType => GeneralTestAnswersType.AudioOnly; }

    public sealed record AudioAndText(string Audio, string Text) : GeneralTestAnswerTypeSpecificData
    { public override GeneralTestAnswersType MatchingEnumType => GeneralTestAnswersType.AudioAndText; }
}
