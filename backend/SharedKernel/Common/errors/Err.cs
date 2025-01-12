using System.Text;

namespace SharedKernel.Common.errors;

public class Err
{
    public string Message { get; init; }
    public ushort Code { get; init; }
    public string? Details { get; init; }
    public ErrorSource Source { get; init; }

    public Err(
        string message,
        ushort code = ErrCodes.Unspecified,
        string? details = null,
        ErrorSource source = ErrorSource.NotSpecified
    ) {
        Message = message;
        Code = code;
        Details = details;
        Source = source;
    }
    public override string ToString() {
        var sb = new StringBuilder();
        if (Code != ErrCodes.Unspecified) {
            sb.AppendLine($"Code: {Code}");
        }
        sb.AppendLine($"Message: {Message}");

        if (!string.IsNullOrEmpty(Details)) {
            sb.AppendLine($"Details: {Details}");
        }

        return sb.ToString();
    }

    public static class ErrFactory
    {

        public static Err NotImplemented(string message = "Not Implemented", string details = "", ErrorSource source = ErrorSource.Server) =>
            new Err(message, ErrCodes.NotImplemented, details, source);
        public static Err NotFound(string message = "Not Found", string details = "", ErrorSource source = ErrorSource.Server) =>
            new Err(message, ErrCodes.NotFound, details, source);

        public static Err Unauthorized(string message = "Unauthorized Access", string details = "", ErrorSource source = ErrorSource.Client) =>
            new Err(message, ErrCodes.UnauthorizedAccess, details, source);
        public static Err InvalidData(string message = "Invalid Data", string details = "", ErrorSource source = ErrorSource.Client) =>
            new Err(message, ErrCodes.InvalidData, details, source);
        public static Err NoAccess(string message = "Access is denied", string details = "", ErrorSource source = ErrorSource.Client) =>
            new Err(message, ErrCodes.NoAccess, details, source);
    }
    public static class ErrCodes
    {
        public const ushort Unspecified = 0;
        public const ushort NotImplemented = 1;

        public const ushort NotFound = 1001;
        public const ushort UnauthorizedAccess = 1002;
        public const ushort InvalidData = 1003;
        public const ushort NoAccess = 1004;
    }

}
