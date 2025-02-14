using SharedKernel.Common.common_enums;
using SharedKernel.Common.tests.value_objects;

namespace SharedKernel.Common.tests.formats_shared.interaction_access_settings;

public interface ITestInteractionsAccessSettings
{
    public AccessLevel TestAccess { get; }
    public ResourceAvailabilitySetting AllowRatings { get; }
    public ResourceAvailabilitySetting AllowComments { get; }
    public bool AllowTestTakenPosts { get; }
    public ResourceAvailabilitySetting AllowTagsSuggestions { get; }
}