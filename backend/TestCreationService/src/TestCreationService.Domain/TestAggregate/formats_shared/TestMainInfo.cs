using SharedKernel.Common;
using SharedKernel.Common.common_enums;
using SharedKernel.Common.constants_store_classes;
using SharedKernel.Common.errors;
using System.Xml.Linq;
using TestCreationService.Domain.Common.rules;
using TestCreationService.Domain.Rules;

namespace TestCreationService.Domain.TestAggregate.formats_shared;

public class TestMainInfo : ValueObject
{
    private TestMainInfo() { }
    public string Name { get; private set; }
    public string CoverImg { get; private set; }
    public string Description { get; private set; }
    public Language Language { get; private set; }

    public override IEnumerable<object> GetEqualityComponents() =>
        [Name, CoverImg, Description, Language];
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
    public ErrOrNothing UpdateCoverImg(string coverImg) {
        int len = string.IsNullOrWhiteSpace(coverImg) ? 0 : coverImg.Length;
        if (len == 0) {
            return Err.ErrFactory.InvalidData("Cover image cannot be empty");
        }
        return ErrOrNothing.Nothing;
    }
}
