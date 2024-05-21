namespace OrderService.IntegrationTests.TestSuite;
public class WebAppFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private MongoDbRunner _mongoDbRunner;
    public Mock<IEventBus> MockEventBus { get; private set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.



    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var applicationAssembly = typeof(TProgram).Assembly;
        var policyNames = new List<string> { "catalog:read", "catalog:write" };

        builder.ConfigureAppConfiguration((context, config) =>
        {
            config.Sources.Clear();
            config.AddJsonFile("appsettings.Test.json", optional: false, reloadOnChange: true);
            config.AddEnvironmentVariables();
        });
        builder.ConfigureServices((context, services) =>
        {
            // Build the service provider to access the configuration
            var sp = services.BuildServiceProvider();
            var config = sp.GetRequiredService<IConfiguration>();

            _mongoDbRunner = MongoDbRunner.StartForDebugging();
            services.AddMongoDbContext<MongoDbContext>(config);

            // Remove the existing IEventBus registration
            var descriptors = services.Where(d => d.ServiceType == typeof(IEventBus)).ToList();
            foreach (var item in descriptors)
            {
                services.Remove(item);
            }

            // Add a mock IEventBus
            MockEventBus = new Mock<IEventBus>();
            services.AddSingleton(MockEventBus.Object);
        });
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _mongoDbRunner.Dispose();
        }
        base.Dispose(disposing);
    }

}
