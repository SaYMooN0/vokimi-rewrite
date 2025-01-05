namespace ApiShared.interfaces;

public interface IRequestWithValidationNeeded
{
    public RequestValidationResult Validate();
}
