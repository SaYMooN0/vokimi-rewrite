using ApiShared.interfaces;
using SharedKernel.Common.domain;
using SharedKernel.Common.errors;

namespace TestTakingService.Api.Contracts.general_format_test.test_taken;

internal class GeneralTestTakenRequest(
    Dictionary<string, string[]> ChosenAnswers,
    GeneralTestTakenRequestFeedbackData? Feedback
) : IRequestWithValidationNeeded
{
    private const int _maxChosenAnswersEntriesCount = 100;

    public RequestValidationResult Validate() {
        if (ChosenAnswers is null || !ChosenAnswers.Any()) {
            return new ErrList(Err.ErrFactory.InvalidData(
                "Chosen answers are not provided")
            );
        }

        if (ChosenAnswers.Count > _maxChosenAnswersEntriesCount) {
            return new ErrList(Err.ErrFactory.InvalidData(
                "Too many questions provided in the chosen answers map")
            );
        }

        ErrList errs = new();

        foreach (var (qId, answerIds) in ChosenAnswers) {
            if (!Guid.TryParse(qId, out _)) {
                errs.Add(Err.ErrFactory.InvalidData(
                    "Invalid chosen answers entry. Incorrect question Id",
                    $"Provided value: {qId}"
                ));
                continue;
            }

            if (answerIds.Length > _maxChosenAnswersEntriesCount) {
                errs.Add(Err.ErrFactory.InvalidData(
                    "Too many answers provided as chosen for the question",
                    $"Question id: {qId}"));
                continue;
            }

            HashSet<string> uniqueAnswers = new();
            foreach (var aId in answerIds) {
                if (!Guid.TryParse(aId, out _)) {
                    errs.Add(Err.ErrFactory.InvalidData(
                        "Invalid chosen answers entry. Incorrect chosen answer Id",
                        $"Invalid answer id: {aId} for question with Id: {qId}"
                    ));
                    continue;
                }

                if (!uniqueAnswers.Add(aId)) {
                    errs.Add(Err.ErrFactory.InvalidData(
                        "Duplicate answer detected for the same question",
                        $"Duplicate answer id: {aId} for question with Id: {qId}"
                    ));
                }
            }
        }

        return errs;
    }

    private Dictionary<GeneralTestQuestionId, HashSet<GeneralTestAnswerId>> ParsedChosenAnswers() {
        return ChosenAnswers.ToDictionary(
            kvp => new GeneralTestQuestionId(Guid.Parse(kvp.Key)),
            kvp => kvp.Value.Select(aId => new GeneralTestAnswerId(Guid.Parse(aId))).ToHashSet()
        );
    }
}

