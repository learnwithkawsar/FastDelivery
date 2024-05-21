namespace FastDelivery.Service.Tracking.Application.Trackings.Dtos;
public class ParcelTrackingHistoryDto
{
    public required string ParcelId { get; set; }
    public required string InvoiceId { get; set; }
    public required string TrackingStatus { get; set; }
}
