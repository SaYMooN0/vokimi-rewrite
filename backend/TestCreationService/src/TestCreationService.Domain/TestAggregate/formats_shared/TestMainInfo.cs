using SharedKernel.Common.common_enums;
using SharedKernel.Common.constants;
using SharedKernel.Common.domain;
using SharedKernel.Common.errors;
using TestCreationService.Domain.Rules;

namespace TestCreationService.Domain.TestAggregate.formats_shared;

public class TestMainInfo : ValueObject
{
    private TestMainInfo() { }
    public string Name { get; private set; }
    public string CoverImg { get; private set; }
    public string Description { get; private set; }
    public Language Language { get; private set; }

    public override IEnumerable<object> GetEqualityComponents() {
        yield return Name;
        yield return CoverImg;
        yield return Description;
        yield return Language;
    }
    public static ErrOr<TestMainInfo> CreateNew(string testName) {
        if (TestRules.CheckTestNameForErrs(testName).IsErr(out var err)) {
            return err;
        }
        return new TestMainInfo() {
            Name = testName,
            CoverImg = ImgOperationsConsts.DefaultTestCoverImg,
            Description = string.Empty,
            Language = Language.Other
        };
    }
    public ErrOrNothing Update(string testName, string description, Language language) {
        if (TestRules.CheckTestNameForErrs(testName).IsErr(out var err)) {
            return err;
        }
        if (TestRules.CheckDescriptionForErrs(description).IsErr(out err)) {
            return err;
        }
        Name = testName;
        Description = description;
        Language = language;
        return ErrOrNothing.Nothing;
    }
    public ErrOrNothing UpdateCoverImg(string coverImg) => TestRules.CheckCoverStringForErrs(coverImg);
    public IEnumerable<Err> CheckForPublishingProblems() {
        if (TestRules.CheckTestNameForErrs(Name).IsErr(out var err)) { yield return err; }
        if (TestRules.CheckDescriptionForErrs(Description).IsErr(out err)) { yield return err; }
        if (TestRules.CheckCoverStringForErrs(CoverImg).IsErr(out err)) { yield return err; }
    }
}
