using FastDelivery.Service.Tracking.Application.Trackings.Dtos;
using Mapster;
using MediatR;

namespace FastDelivery.Service.Tracking.Application.Trackings.Features;
public record GetParcelHistoryQuery(string invoiceId) : IRequest<List<ParcelTrackingHistoryDto>> { }

public class GetParcelHistoryQueryHandler : IRequestHandler<GetParcelHistoryQuery, List<ParcelTrackingHistoryDto>>
{
    private readonly ITrackingRepository _trackingRepository;
    public GetParcelHistoryQueryHandler(ITrackingRepository trackingRepository)
    {
        _trackingRepository = trackingRepository;
    }
    public async Task<List<ParcelTrackingHistoryDto>> Handle(GetParcelHistoryQuery request, CancellationToken cancellationToken)
    {
        var data = await _trackingRepository.FindAsync(e => e.InvoiceId == request.invoiceId, cancellationToken);

        return data.Adapt<List<ParcelTrackingHistoryDto>>();

    }
}

