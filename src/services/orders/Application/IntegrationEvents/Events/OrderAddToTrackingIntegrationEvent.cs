namespace FastDelivery.Service.Order.Application.IntegrationEvents.Events;
public record OrderAddToTrackingIntegrationEvent(ParcelDto ParcelDto) : IntegrationEvent;