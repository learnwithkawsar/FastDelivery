using FastDelivery.Service.Order.Application.Parcels.Dtos;
using FastDelivery.Service.Order.Application.Parcels.Features;
using FastDelivery.Service.Tracking.Application.Trackings.Dtos;
using FastDelivery.Service.Tracking.Application.Trackings.Features;
using Microsoft.AspNetCore.Mvc;

namespace TrackingService.Api.Controllers;

public class TrackingsController : VersionedApiController
{
    [HttpPost("{id}", Name = nameof(GetHistoryAsync))]
    [ProducesResponseType(201, Type = typeof(List<ParcelTrackingHistoryDto>))]
    public async Task<List<ParcelTrackingHistoryDto>> GetHistoryAsync(string id)
    {
        try
        {
            return await Mediator.Send(new GetParcelHistoryQuery(id));
        }
        catch (Exception ex)
        {
            throw;
        }


    }
}
