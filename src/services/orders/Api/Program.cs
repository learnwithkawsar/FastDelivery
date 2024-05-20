using FastDelivery.Service.Order.Infrastructure;

namespace OrderService.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddOrderInfrastructure();
        var app = builder.Build();
        app.MapSubscribeHandler();
        app.UseOrderInfrastructure();
        app.MapGet("/", () => "Percel Service is Running!!");
        app.Run();
    }
}
