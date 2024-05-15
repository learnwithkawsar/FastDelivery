using FastDelivery.Framework.Core.Domain;

namespace FastDelivery.Service.Tracking.Domain;

public class ParcelTrackingHistory : BaseEntity
{
    public required string ParcelId { get; set; }
    public required string InvoiceId { get; set; }
    public required string  TrackingStatus { get; set; }
}
