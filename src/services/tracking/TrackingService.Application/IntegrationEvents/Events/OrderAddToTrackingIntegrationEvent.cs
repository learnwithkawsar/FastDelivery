using FastDelivery.BuildingBlocks.EventBus.Events;
namespace FastDelivery.Service.Tracking.Application.IntegrationEvents.Events;
public record OrderAddToTrackingIntegrationEvent(ParcelDto ParcelDto) : IntegrationEvent;

