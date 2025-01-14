using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel.Common.common_enums;
using SharedKernel.Common.tests;

namespace TestCreationService.Infrastructure.Persistence.configurations.value_converters;

internal class ResourceAvailabilitySettingConverter : ValueConverter<ResourceAvailabilitySetting, string>
{
    public ResourceAvailabilitySettingConverter() : base(
        v => v.IsEnabled ? v.Access.ToString() : "Disabled",
        v => v == "Disabled"
            ? ResourceAvailabilitySetting.Disabled
            : new ResourceAvailabilitySetting(true, Enum.Parse<AccessLevel>(v))
    ) {
    }
}
