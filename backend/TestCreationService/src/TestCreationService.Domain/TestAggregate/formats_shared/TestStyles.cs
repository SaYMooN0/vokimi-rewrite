
using SharedKernel.Common;

namespace TestCreationService.Domain.TestAggregate.formats_shared;

public class TestStyles : ValueObject
{
    public static TestStyles Default => new ();

    // how test looks in the catalog
    //---View test page
    // how test cover looks like
    // how underlying buttons buttons look like
    //---Test taking page
    //
    //---View results page
    public override IEnumerable<object> GetEqualityComponents() {
        yield return new object();
    }
}
