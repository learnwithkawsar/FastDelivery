using FastDelivery.Framework.Core.Services;

namespace FastDelivery.Framework.Infrastructure.Services;
public class DateTimeService : IDateTimeService
{
    public DateTime DateTimeUtcNow => DateTime.UtcNow;
    public DateOnly DateOnlyUtcNow => DateOnly.FromDateTime(DateTimeUtcNow);
}
