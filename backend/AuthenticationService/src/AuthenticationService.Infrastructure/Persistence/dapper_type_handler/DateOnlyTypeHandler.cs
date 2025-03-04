using System.Data;
using Dapper;
using NpgsqlTypes;

namespace AuthenticationService.Infrastructure.Persistence.dapper_type_handler;

public class DateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
{
    public override void SetValue(IDbDataParameter parameter, DateOnly value) {
        if (parameter is Npgsql.NpgsqlParameter npgsqlParameter) {
            npgsqlParameter.NpgsqlDbType = NpgsqlDbType.Date;
        }
        parameter.Value = value.ToDateTime(TimeOnly.MinValue);
    }

    public override DateOnly Parse(object value) {
        return value switch {
            DateTime dateTime => DateOnly.FromDateTime(dateTime),
            _ => throw new InvalidCastException($"Cannot convert '{value}' to DateOnly.")
        };
    }
}