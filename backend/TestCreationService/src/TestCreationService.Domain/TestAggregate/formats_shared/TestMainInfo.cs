using SharedKernel.Common;
using SharedKernel.Common.common_enums;
using SharedKernel.Common.constants_store_classes;
using SharedKernel.Common.errors;
using TestCreationService.Domain.Common.rules;

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
        if (IsStringCorrectTestName(testName).IsErr(out var testNameErr)) {
            return testNameErr;
        }
        return new TestMainInfo() {
            Name = testName,
            CoverImg = ImgOperationsConsts.DefaultTestCoverImg,
            Description = string.Empty,
            Language = Language.Other
        };
    }
    public ErrOrNothing Update(string name, string description, Language language) {
        if (IsStringCorrectTestName(name).IsErr(out var testNameErr)) {
            return testNameErr;
        }
        if (IsStringCorrectTestDescription(description).IsErr(out var descriptionErr)) {
            return descriptionErr;
        }
        Name = name;
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
    private static ErrOrNothing IsStringCorrectTestName(string str) {
        int len = string.IsNullOrWhiteSpace(str) ? 0 : str.Length;
        if (len < TestRules.MinNameLength || len > TestRules.MaxNameLength) {
            return Err.ErrFactory.InvalidData($"Test name length must be between {TestRules.MinNameLength} and {TestRules.MaxNameLength} characters. Current length: {len}");
        }
        return ErrOrNothing.Nothing;

    }
    private static ErrOrNothing IsStringCorrectTestDescription(string str) {
        int len = string.IsNullOrWhiteSpace(str) ? 0 : str.Length;
        if (len > TestRules.MaxTestDescriptionLength) {
            return Err.ErrFactory.InvalidData($"Test description must be less then {TestRules.MaxTestDescriptionLength} characters. Current length: {len}");
        }
        return ErrOrNothing.Nothing;

    }
}
