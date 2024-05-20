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
        builder.ConfigureServices((context, services) =>
        {
            // Build the service provider to access the configuration
            var sp = services.BuildServiceProvider();
            var config = sp.GetRequiredService<IConfiguration>();

            // Remove existing service registrations if necessary
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IValidator<ParcelInfo>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Add mock services
            var mockValidator = new Mock<IValidator<ParcelInfo>>();
            mockValidator.Setup(v => v.Validate(It.IsAny<ParcelInfo>())).Returns(new ValidationResult());
            services.AddSingleton(mockValidator.Object);

            // Register FluentValidation
            services.AddValidatorsFromAssemblyContaining<Vailator>();

            // Register MediatR
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<AddParcelCommandHandler>());

            // Register other dependencies
            services.AddCustomeApiVersioning();
            services.AddExceptionMiddleware();
            services.AddControllers().AddDapr();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });
            services.AddRouting(options => options.LowercaseUrls = true);

            var applicationAssembly = typeof(TProgram).Assembly;
            services.AddMapsterExtension(applicationAssembly);
            services.AddBehaviors();
            services.AddValidatorsFromAssembly(applicationAssembly);
            services.AddMediatR(o => o.RegisterServicesFromAssembly(applicationAssembly));
            services.AddInternalServices();
            services.AddDaprClient();
            services.AddHttpLogging(logging =>
            {
                logging.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All;
                logging.RequestHeaders.Add(HeaderNames.Accept);
                logging.RequestHeaders.Add(HeaderNames.ContentType);
                logging.RequestHeaders.Add(HeaderNames.ContentDisposition);
                logging.RequestHeaders.Add(HeaderNames.ContentEncoding);
                logging.RequestHeaders.Add(HeaderNames.ContentLength);

                logging.MediaTypeOptions.AddText("application/json");
                logging.MediaTypeOptions.AddText("multipart/form-data");

                logging.RequestBodyLogLimit = 4096;
                logging.ResponseBodyLogLimit = 4096;
            });
            _mongoDbRunner = MongoDbRunner.StartForDebugging();

            services.AddMongoDbContext<MongoDbContext>(config);
            // services.AddTransient<IParcelRepository, ParcelRepository>();

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
