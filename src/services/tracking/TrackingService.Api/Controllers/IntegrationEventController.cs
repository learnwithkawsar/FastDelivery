using Dapr;
using FastDelivery.Service.Tracking.Application.IntegrationEvents.EventHandling;
using FastDelivery.Service.Tracking.Application.IntegrationEvents.Events;
using static FastDelivery.Framework.Core.Constants.DaprConstants;
using Microsoft.AspNetCore.Mvc;

namespace TrackingService.Api.Controllers;
[Route("api/v1/[controller]")]
[ApiController]
public class IntegrationEventController : ControllerBase
{    

    [HttpPost("OrderAddToTracking")]
    [Topic(DAPR_PUBSUB_NAME, nameof(OrderAddToTrackingIntegrationEvent))]
    public Task HandleAsync(
        OrderAddToTrackingIntegrationEvent @event,
        [FromServices] OrderAddToTrackingIntegrationEventHandler handler) =>
        handler.Handle(@event);
}
