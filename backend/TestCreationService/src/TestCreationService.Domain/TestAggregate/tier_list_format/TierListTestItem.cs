using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;
using SharedKernel.Common.tests.tier_list_format;

namespace TestCreationService.Domain.TestAggregate.tier_list_format;

public class TierListTestItem : Entity<TierListTestItemId>
{
    private TierListTestItem() { }
    public string Name { get; private set; }

    public string? Clarification { get; private set; }
    public TierListTestItemContentData Content { get; private set; }

    public static ErrOr<TierListTestItem> CreateNew(
        string name, string? clarification,
        TierListTestItemContentData content
    ) {
        if (TierListTestItemRules.CheckIfStringCorrectItemName(name).IsErr(out var err)) {
            return err;
        }

        if (TierListTestItemRules.CheckIfStringCorrectItemClarification(clarification).IsErr(out err)) {
            return err;
        }

        return new TierListTestItem() {
            Name = name,
            Clarification = clarification,
            Content = content
        };
    }

    public ErrOrNothing Update(
        string newName, string? newClarification,
        TierListTestItemContentData newContent
    ) {
        if (newContent.MatchingEnumType != this.Content.MatchingEnumType) {
            return new Err(
                "Incorrect content type. New content type must be the same as previous",
                details:
                $"Previous content type: {this.Content.MatchingEnumType}. New content type: {newContent.MatchingEnumType}"
            );
        }

        if (TierListTestItemRules.CheckIfStringCorrectItemName(newName).IsErr(out var err)) {
            return err;
        }

        if (TierListTestItemRules.CheckIfStringCorrectItemClarification(newClarification).IsErr(out err)) {
            return err;
        }

        this.Name = newName;
        this.Clarification = newClarification;
        this.Content = newContent;
        return ErrOrNothing.Nothing;
    }
}