using AuthenticationService.Domain.Common.value_objects;
using Dapper;
using System.Data;

namespace AuthenticationService.Infrastructure.Persistence.dapper_type_handler;

internal class EmailTypeHandler : SqlMapper.TypeHandler<Email>
{
    public override void SetValue(IDbDataParameter parameter, Email value) {
        parameter.Value = value.ToString();  
    }

    public override Email Parse(object value) {
        if (value is string str) {
            var result = Email.Create(str);
            if (result.IsErr(out var err)) {
                throw new ArgumentException("Invalid email format", nameof(value));
            }
            return result.GetSuccess();
        }
        throw new ArgumentException("Invalid email value", nameof(value));
    }
}