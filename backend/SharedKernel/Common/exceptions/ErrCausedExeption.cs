using SharedKernel.Common.errors;

namespace SharedKernel.Common.exceptions;

public class ErrCausedException : Exception
{
    public Err Err { get; set; }
    private ErrCausedException() { }
    public ErrCausedException(Err err) { Err = err; }
}
