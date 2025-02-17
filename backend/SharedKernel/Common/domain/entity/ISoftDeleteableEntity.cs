using SharedKernel.Common.errors;
using SharedKernel.Common.interfaces;

namespace SharedKernel.Common.domain.entity;

public interface ISoftDeleteableEntity
{
    public bool IsDeleted { get; }
    public DateTime? DeletedAt { get; }
    public ErrOrNothing Delete(IDateTimeProvider timeProvider);
}