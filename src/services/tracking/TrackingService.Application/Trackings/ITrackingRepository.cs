using FastDelivery.Framework.Core.Database;
using FastDelivery.Service.Tracking.Domain;

namespace FastDelivery.Service.Tracking.Application.Trackings;
public interface ITrackingRepository : IRepository<ParcelTrackingHistory, Guid>
{

}
