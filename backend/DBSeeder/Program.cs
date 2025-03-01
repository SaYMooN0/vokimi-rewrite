using TestCatalogService.Infrastructure.Persistence;
public class Program
{
    public static void Main(string[] args) {
        string dbConnetionString = configuration.GetConnectionString("TestCatalogServiceDb")
                                   ?? throw new Exception("Database connection string is not provided.");
        services.AddDbContext<TestCatalogDbContext>(options => options.UseNpgsql(dbConnetionString));
    }
}