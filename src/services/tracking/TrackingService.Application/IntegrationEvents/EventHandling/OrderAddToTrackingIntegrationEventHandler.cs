using FastDelivery.BuildingBlocks.EventBus.Abstractions;
using FastDelivery.Service.Tracking.Application.IntegrationEvents.Events;
using FastDelivery.Service.Tracking.Application.Trackings.Features;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FastDelivery.Service.Tracking.Application.IntegrationEvents.EventHandling;
public class OrderAddToTrackingIntegrationEventHandler : IIntegrationEventHandler<OrderAddToTrackingIntegrationEvent>
{
    private readonly ILogger<OrderAddToTrackingIntegrationEventHandler> _logger;
    private readonly ISender _mediatR;

    public OrderAddToTrackingIntegrationEventHandler(ILogger<OrderAddToTrackingIntegrationEventHandler> logger, ISender mediatR)
    {
        _logger = logger;
        _mediatR = mediatR;
    }

    public async Task Handle(OrderAddToTrackingIntegrationEvent @event)
    {
        var req = new AddTrackingHistoryCommand(@event.ParcelDto.ParcelId!, @event.ParcelDto.InvoiceId!, "Placed");
        await _mediatR.Send(req);
        _logger.LogInformation($"Event Completed : {nameof(OrderAddToTrackingIntegrationEvent)}");
        await Task.Delay(3000);
    }
}
