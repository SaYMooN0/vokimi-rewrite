namespace ApiShared.middlewares.request_validation;

public class BodyToValidateTypeMetaData
{
    public Type RequestType { get; init; }

    public BodyToValidateTypeMetaData(Type requestType) {
        RequestType = requestType;
    }
}
