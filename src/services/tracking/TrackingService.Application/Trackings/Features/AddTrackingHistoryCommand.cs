using FastDelivery.Service.Tracking.Domain;
using MediatR;

namespace FastDelivery.Service.Tracking.Application.Trackings.Features;
public record AddTrackingHistoryCommand(string ParcelId, string InvoiceId, string TrackingStatus) : IRequest<Guid>
{ }

public class AddTrackingHistoryCommandHandler : IRequestHandler<AddTrackingHistoryCommand, Guid>
{
    private readonly ITrackingRepository _trackingRepository;

    public AddTrackingHistoryCommandHandler(ITrackingRepository trackingRepository)
    {
        _trackingRepository = trackingRepository;

    }

    public async Task<Guid> Handle(AddTrackingHistoryCommand request, CancellationToken cancellationToken)
    {
        var parcelTrackingHistory = new ParcelTrackingHistory()
        {
            InvoiceId = request.InvoiceId,
            ParcelId = request.ParcelId,
            TrackingStatus = request.TrackingStatus,
        };
        await _trackingRepository.AddAsync(parcelTrackingHistory, cancellationToken);

        return parcelTrackingHistory.Id;
    }
}
