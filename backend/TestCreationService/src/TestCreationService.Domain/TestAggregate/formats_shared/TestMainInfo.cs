using SharedKernel.Common;
using SharedKernel.Common.common_enums;
using SharedKernel.Common.constants_store_classes;
using SharedKernel.Common.errors;
using TestCreationService.Domain.Rules;

namespace TestCreationService.Domain.TestAggregate.formats_shared;

public class TestMainInfo : ValueObject
{
    private TestMainInfo() { }
    public string Name { get; private set; }
    public string CoverImg { get; private set; }
    public string? Description { get; private set; }
    public Language Language { get; private set; }

    public override IEnumerable<object> GetEqualityComponents() =>
        [Name, CoverImg, Description, Language];
    public static ErrOr<TestMainInfo> CreateNew(string testName) {
        if (IsStringCorrectTestName(testName).IsErr(out var testNameErr)) {
            return testNameErr;
        }
        return new TestMainInfo() {
            Name = testName,
            CoverImg = ImgOperationsConsts.DefaultTestCoverImg,
            Description = null,
            Language = Language.Other
        };
    }
    public ErrOrNothing Update(string name, string? description, Language language) {
        if (IsStringCorrectTestName(name).IsErr(out var testNameErr)) {
            return testNameErr;
        }
        Name = name;
        Description = description;
        Language = language;
        return ErrOrNothing.Nothing;
    }
    public void UpdateCoverImg(string coverImg) {
        CoverImg = coverImg;
        //domain event? ...
    }
    private static ErrOrNothing IsStringCorrectTestName(string str) {
        int len = string.IsNullOrWhiteSpace(str) ? 0 : str.Length;
        if (len < TestRules.MinNameLength || len > TestRules.MaxNameLength) {
            return new Err($"Test name length mus be between {TestRules.MinNameLength} and {TestRules.MaxNameLength} characters. Current length: {len}");
        }
        return ErrOrNothing.Nothing;

    }
}
