using Dapper;
using SharedKernel.Common.EntityIds;
using System.Data;

namespace AuthenticationService.Infrastructure.Persistence.dapper_type_handler;
internal class GuidEntityIdTypeHandler<T> : SqlMapper.TypeHandler<T> where T : EntityId
{
    public override void SetValue(IDbDataParameter parameter, T value) {
        parameter.Value = value.Value;
    }

    public override T Parse(object value) {
        if (value is Guid guidValue) {
            return (T)Activator.CreateInstance(typeof(T), guidValue);
        }

        throw new ArgumentException($"Invalid GUID value for {typeof(T).Name}", nameof(value));
    }
}
