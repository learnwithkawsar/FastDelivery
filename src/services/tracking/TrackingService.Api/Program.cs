using FastDelivery.Service.Tracking.Infrastructure;
namespace TrackingService.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddTrackingInfrastructure();
        var app = builder.Build();
        app.UseTrackingInfrastructure();

        app.Run();
    }
}
