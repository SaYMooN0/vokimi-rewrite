namespace DBSeeder;

public class DbContextSeederException : Exception
{
    public Type SeederType { get; }

    public DbContextSeederException(
        Exception innerException,
        Type seederType
    ) : base($"Exception in {seederType}", innerException) {
        SeederType = seederType;
    }
}