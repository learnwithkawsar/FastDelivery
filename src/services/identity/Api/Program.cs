
using FastDelivery.Service.Identity.Infrastructure;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.AddIdentityInfrastructure();
            var app = builder.Build();
            app.UseIdentityInfrastructure();
            app.MapGet("/", () => "Identity Service is Running!!");
            app.Run();
        }
    }
}
